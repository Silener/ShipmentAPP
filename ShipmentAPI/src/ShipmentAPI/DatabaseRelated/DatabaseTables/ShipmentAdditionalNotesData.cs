using System.ComponentModel.DataAnnotations.Schema;
using ShipmentCommonClasses.CommonDtos;

namespace ShipmentAPI.DatabaseTables
{
    [Table("SHIPMENT_ADDITIONAL_NOTES")]
    public class ShipmentAdditionalNotes
    {
        public void SetFields(ShipmentDetails shipmentDetails)
        {
            this.shipmentId = shipmentDetails.shipmentId;
            this.additionalNotes = shipmentDetails.shipmentAdditionalNotes;
        }

        [Column("ID")]
        public int Id { get; set; }

        [Column("SHIPMENT_ID")]
        public int shipmentId { get; set; }

        [Column("ADDITIONAL_NOTES")]
        public string? additionalNotes { get; set; }
    }
}