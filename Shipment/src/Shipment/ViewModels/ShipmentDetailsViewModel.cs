using System.ComponentModel;
using System.Web.Mvc;
using ShipmentCommonClasses.CommonDtos;

namespace Shipment.ViewModels
{
    public class ShipmentDetailsViewModel
    {
        public ShipmentDetails shipmentDetails { get; set; }

        [DisplayName("Shipment Status")]
        public string shipmentStatus { get; set; }

        public List<string> validationMessages { get; set; }
    }
}