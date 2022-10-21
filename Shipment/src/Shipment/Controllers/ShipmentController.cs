using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Shipment.ViewModels;
using Shipment.Validators;
using ShipmentCommonClasses.Dtos;
using ShipmentCommonClasses.CommonDtos;
using ShipmentCommonClasses;
using ShipmentCommonClasses.Enums;

namespace Shipment.Controllers
{
    public class ShipmentController : Controller
    {
        public readonly int pageSize = 25;

        public ShipmentController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? shipmentStatus = 1, int? truckId = 1, int? page = 1)
        {
            List<ShipmentListData> shipmentList = new List<ShipmentListData>();
            BackendAPIResolver<DetailedDataModel> backendAPIResolver = new BackendAPIResolver<DetailedDataModel>();

            try
            {
                // Make a get request
                DetailedDataModel detailedDataModel = await backendAPIResolver.GetRequest($"/api/{page}/{truckId}/{shipmentStatus}");

                shipmentList = detailedDataModel.detailedData;

                // Set to pagedList
                var resultAsPagedList = new StaticPagedList<ShipmentListData>(shipmentList, (int)page, pageSize, detailedDataModel.recordsCount);

                return View(resultAsPagedList);

            }
            catch (HttpRequestException httpRequestException)
            {
                return BadRequest($"Error getting shipments list: {httpRequestException.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? Id)
        {
            ShipmentDetailsViewModel shipmentDetailsViewModel = new ShipmentDetailsViewModel();
            BackendAPIResolver<ShipmentDetails> backendAPIResolver = new BackendAPIResolver<ShipmentDetails>();

            try
            {
                // Perform get request
                ShipmentDetails shipmentDetails = await backendAPIResolver.GetRequest($"/api/details/{Id}");

                shipmentDetailsViewModel.shipmentDetails = shipmentDetails;

                // Take shipment status from dictionary
                shipmentDetailsViewModel.shipmentStatus = Dictionaries.shipmentStatuses[shipmentDetailsViewModel.shipmentDetails.shipmentStatus];

                return View(shipmentDetailsViewModel);
            }
            catch (HttpRequestException httpRequestException)
            {
                return BadRequest($"Error getting shipment details: {httpRequestException.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Deliver(ShipmentDetailsViewModel shipment)
        {
            // Make status to deliver and finalize
            shipment.shipmentDetails.shipmentStatus = ShipmentStatus.ShipmentStatusDelivered;
            return await FinalizeOrder(shipment, true);
        }

        [HttpPost]
        public async Task<ActionResult> Hold(ShipmentDetailsViewModel shipment)
        {
            // Make status to Held-in-truck and finalize
            shipment.shipmentDetails.shipmentStatus = ShipmentStatus.ShipmentStatusHeld;
            return await FinalizeOrder(shipment, false);
        }

        public async Task<ActionResult> FinalizeOrder(ShipmentDetailsViewModel shipment, bool isMarkedAsDelivered)
        {
            if (shipment.validationMessages != null)
                shipment.validationMessages.Clear();

            // Make validations
            DeliveryFinalizeValidators deliveryFinalizeValidator = new DeliveryFinalizeValidators();
            shipment.validationMessages = deliveryFinalizeValidator.Validate(shipment);

            if (shipment.validationMessages.Count() > 0)
            {
                shipment.shipmentDetails.shipmentStatus = ShipmentStatus.ShipmentStatusOutForDelivery;
                return View("Details", shipment);
            }

            // Call backend with post
            BackendAPIResolver<ShipmentDetails> backendAPIResolver = new BackendAPIResolver<ShipmentDetails>();

            shipment.shipmentDetails.shipmentAdditionalNotes = shipment.shipmentDetails.shipmentAdditionalNotes == null
            ? "" : shipment.shipmentDetails.shipmentAdditionalNotes;

            HttpResponseMessage httpResponse = await backendAPIResolver.PostRequest("shipmentDetails", "/api/update/shipmentDetails", shipment.shipmentDetails);

            if (!httpResponse.IsSuccessStatusCode)
                return StatusCode(500);

            shipment.shipmentStatus = Dictionaries.shipmentStatuses[shipment.shipmentDetails.shipmentStatus];

            return View("Details", shipment);
        }

        [HttpPost]
        public async Task<ActionResult> Undo(ShipmentDetailsViewModel shipment)
        {
            shipment.shipmentDetails.shipmentAdditionalNotes = shipment.shipmentDetails.shipmentAdditionalNotes == null
            ? "" : shipment.shipmentDetails.shipmentAdditionalNotes;

            // Call the web api with undo
            BackendAPIResolver<ShipmentDetails> backendAPIResolver = new BackendAPIResolver<ShipmentDetails>();
            HttpResponseMessage httpResponse = await backendAPIResolver.PostRequest("undoShipment", "/api/update/undoShipment", shipment.shipmentDetails);

            if (!httpResponse.IsSuccessStatusCode)
                return StatusCode(500);

            // Reset view model
            shipment.shipmentDetails.customerAcceptance = CustomerShipmentAcceptanceStatus.StatusUndefined;
            shipment.shipmentDetails.packageIntegrity = PackageIntegrity.IntegrityUndefined;
            shipment.shipmentDetails.shipmentAdditionalNotes = "";
            shipment.shipmentDetails.shipmentStatus = ShipmentStatus.ShipmentStatusOutForDelivery;
            shipment.shipmentStatus = Dictionaries.shipmentStatuses[ShipmentStatus.ShipmentStatusOutForDelivery];

            return View("Details", shipment);
        }
    }
}
