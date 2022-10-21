using ShipmentCommonClasses.Enums;
using System.ComponentModel;

namespace ShipmentCommonClasses.CommonDtos
{
    public class ShipmentDetails
    {
        public int shipmentId { get; set; }

        public int truckId { get; set; }

        public ShipmentStatus shipmentStatus { get; set; }

        [DisplayName("Sender Name")]
        public string senderName { get; set; }

        [DisplayName("Sender Address")]
        public string senderAddress { get; set; }

        [DisplayName("Receiver Name")]
        public string receiverName { get; set; }

        [DisplayName("Receiver Address")]
        public string receiverAddress { get; set; }

        [DisplayName("Packages Count")]
        public short packagesCount { get; set; }

        [DisplayName("Tracking Number")]
        public string trackingNumber { get; set; }

        [DisplayName("Shipment Acceptance")]
        public CustomerShipmentAcceptanceStatus customerAcceptance { get; set; }

        [DisplayName("Package Integrity")]
        public PackageIntegrity packageIntegrity { get; set; }

        [DisplayName("Additional Notes")]
        public string shipmentAdditionalNotes { get; set; }
    }
}