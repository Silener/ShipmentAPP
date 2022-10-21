using System.ComponentModel.DataAnnotations.Schema;
using ShipmentCommonClasses.Enums;

namespace ShipmentAPI.DatabaseTables
{
    [Table("SHIPMENTS")]
    public class ShipmentItem
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("TRUCK_ID")]
        public int truckId { get; set; }
        [Column("SHIPMENT_STATUS")]
        public ShipmentStatus shipmentStatus { get; set; }
    }
}