using System.ComponentModel.DataAnnotations.Schema;
using ShipmentCommonClasses.CommonDtos;
using ShipmentCommonClasses.Enums;

namespace ShipmentAPI.DatabaseTables
{
    [Table("POST_SHIPMENT_DATA")]
    public class PostShipmentData
    {

        public void SetFields(ShipmentDetails shipmentDetails)
        {
            this.shipmentId = shipmentDetails.shipmentId;
            this.customerShipmentAcceptanceStatus = shipmentDetails.customerAcceptance;
            this.packageIntegrity = shipmentDetails.packageIntegrity;
        }

        [Column("ID")]
        public int Id { get; set; }

        [Column("SHIPMENT_ID")]
        public int shipmentId { get; set; }

        [Column("CUSTOMER_SHIPMENT_ACCEPTANCE_STATUS")]
        public CustomerShipmentAcceptanceStatus customerShipmentAcceptanceStatus { get; set; }

        [Column("PACKAGE_INTEGRITY")]
        public PackageIntegrity packageIntegrity { get; set; }
    }
}