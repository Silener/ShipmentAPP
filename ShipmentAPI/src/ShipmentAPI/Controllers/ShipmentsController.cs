using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ShipmentAPI.DatabaseTables;
using ShipmentCommonClasses.CommonDtos;
using ShipmentCommonClasses.Dtos;
using ShipmentCommonClasses.Enums;

namespace ShipmentAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ShipmentController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly int pageSize = 25;
    private readonly DatabaseContexts _context;

    private readonly ILogger<ShipmentController> _logger;

    public ShipmentController(ILogger<ShipmentController> logger, DatabaseContexts context, IMemoryCache memoryCache)
    {
        _logger = logger;
        _context = context;
        _memoryCache = memoryCache;
    }

    [HttpGet("/api/{page}/{truckId}/{shipmentStatus}")]
    public async Task<ActionResult<IEnumerable<DetailedDataModel>>> Get(int page = 1, int truckId = 1, int shipmentStatus = 1)
    {
        DetailedDataModel detailedDataModel = new DetailedDataModel();

        var shipments = (from shipment in _context.ShipmentsTable
                         join shipmentDetails in _context.ShipmentDetailedDataTable on shipment.Id equals shipmentDetails.shipmentId
                         where shipment.truckId == truckId
                         select new ShipmentListData
                         {
                             shipmentId = shipment.Id,
                             truckId = shipment.truckId,
                             shipmentStatus = shipment.shipmentStatus,
                             receiverName = shipmentDetails.receiverName,
                             receiverAddress = shipmentDetails.receiverAddress,
                             packagesCount = shipmentDetails.packagesCount
                         }).ToList().Skip((page - 1) * pageSize).Take(pageSize).ToList();

        // We take the count of the rows for the paging
        var count = (from shipment in _context.ShipmentsTable select shipment);

        // We filter by shipment status if we have one provided
        if ((ShipmentStatus)shipmentStatus != ShipmentStatus.ShipmentStatusUndefined)
        {
            shipments = shipments.Where(shipment => shipment.shipmentStatus == (ShipmentStatus)shipmentStatus).ToList();
            count = count.Where(shipment => shipment.shipmentStatus == (ShipmentStatus)shipmentStatus);
        }

        // Building the transport object
        detailedDataModel = new DetailedDataModel();
        detailedDataModel.detailedData = shipments;
        detailedDataModel.recordsCount = count.Count();

        return Ok(detailedDataModel);
    }

    [HttpGet("/api/details/{shipmentId}")]
    public async Task<ActionResult<ShipmentDetails>> Get(int shipmentId)
    {
        // We check if we have the specific shipment details in the cache
        ShipmentDetails detailedShipment = new ShipmentDetails();
        if (!_memoryCache.TryGetValue(shipmentId, out detailedShipment))
        {
            detailedShipment = await (from shipments in _context.ShipmentsTable
                                      join shipmentDetailedData in _context.ShipmentDetailedDataTable on shipments.Id equals shipmentDetailedData.shipmentId
                                      join postShipmentDetails in _context.PostShipmentDataTable on shipments.Id equals postShipmentDetails.shipmentId into ps_postShipmentDetailsJoined
                                      from joinedPostShipmentDetails in ps_postShipmentDetailsJoined.DefaultIfEmpty()
                                      join shipmentAdditionalNotes in _context.ShipmentAdditionalNotesTable on shipments.Id equals shipmentAdditionalNotes.shipmentId into ps_shipmentAdditionalNotes
                                      from joinedShipmentAdditionalNotes in ps_shipmentAdditionalNotes.DefaultIfEmpty()
                                      where shipments.Id == shipmentId
                                      select new ShipmentDetails
                                      {
                                          shipmentId = shipments.Id,
                                          truckId = shipments.truckId,
                                          shipmentStatus = shipments.shipmentStatus,
                                          senderName = shipmentDetailedData.senderName,
                                          senderAddress = shipmentDetailedData.senderAddress,
                                          receiverName = shipmentDetailedData.receiverName,
                                          receiverAddress = shipmentDetailedData.receiverAddress,
                                          packagesCount = shipmentDetailedData.packagesCount,
                                          trackingNumber = shipmentDetailedData.trackingNumber,
                                          customerAcceptance = joinedPostShipmentDetails.customerShipmentAcceptanceStatus == null ? CustomerShipmentAcceptanceStatus.StatusUndefined : joinedPostShipmentDetails.customerShipmentAcceptanceStatus,
                                          packageIntegrity = joinedPostShipmentDetails.packageIntegrity == null ? PackageIntegrity.IntegrityUndefined : joinedPostShipmentDetails.packageIntegrity,
                                          shipmentAdditionalNotes = joinedShipmentAdditionalNotes.additionalNotes == null ? "" : joinedShipmentAdditionalNotes.additionalNotes
                                      }).FirstOrDefaultAsync();

            // If not, we set it.
            _memoryCache.Set(shipmentId, detailedShipment, TimeSpan.FromMinutes(1));
        }

        return Ok(detailedShipment);
    }

    [HttpPut("/api/update/shipmentDetails")]
    public async Task<ActionResult<ShipmentDetails>> Put(ShipmentDetails shipmentDetails)
    {
        try
        {
            // Finding and updating the new main shipment data
            ShipmentItem shipmentMainData = _context.ShipmentsTable.Find(shipmentDetails.shipmentId);
            shipmentMainData.shipmentStatus = shipmentDetails.shipmentStatus;
            _context.ShipmentsTable.Update(shipmentMainData);

            // Add post shipment data if it's filled
            if (shipmentDetails.customerAcceptance != CustomerShipmentAcceptanceStatus.StatusUndefined)
            {
                PostShipmentData postShipmentData = new PostShipmentData();
                postShipmentData.SetFields(shipmentDetails);

                _context.PostShipmentDataTable.Add(postShipmentData);
            }

            // Add additional notes data if it's filled
            if (shipmentDetails.shipmentAdditionalNotes.Length > 0)
            {
                ShipmentAdditionalNotes shipmentAdditionalNotes = new ShipmentAdditionalNotes();
                shipmentAdditionalNotes.SetFields(shipmentDetails);

                _context.ShipmentAdditionalNotesTable.Add(shipmentAdditionalNotes);
            }

            _memoryCache.Remove(shipmentDetails.shipmentId);

            _context.SaveChanges();

        }
        catch
        {
            return StatusCode(500);
        }

        return Ok();
    }

    [HttpPut("/api/update/undoShipment")]
    public async Task<ActionResult<ShipmentDetails>> UndoPut(ShipmentDetails undoShipment)
    {
        try
        {
            // Find the main shipment data and update
            ShipmentItem shipmentMainData = _context.ShipmentsTable.Find(undoShipment.shipmentId);
            shipmentMainData.shipmentStatus = ShipmentStatus.ShipmentStatusOutForDelivery;
            _context.ShipmentsTable.Update(shipmentMainData);

            // Find post shipment data and remove
            PostShipmentData postShipmentData = (from postShipment in _context.PostShipmentDataTable where postShipment.shipmentId == undoShipment.shipmentId select postShipment).FirstOrDefault();
            if (postShipmentData != null)
                _context.PostShipmentDataTable.Remove(postShipmentData);

            // Find additional notes and remove
            ShipmentAdditionalNotes shipmentAdditionalNotes = (from additionalNotes in _context.ShipmentAdditionalNotesTable where additionalNotes.shipmentId == undoShipment.shipmentId select additionalNotes).FirstOrDefault();
            if (shipmentAdditionalNotes != null)
                _context.ShipmentAdditionalNotesTable.Remove(shipmentAdditionalNotes);

            _memoryCache.Remove(undoShipment.shipmentId);

            _context.SaveChanges();

        }
        catch
        {
            return StatusCode(500);
        }

        return Ok();
    }
}
