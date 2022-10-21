using System.ComponentModel.DataAnnotations.Schema;

namespace ShipmentAPI.DatabaseTables
{
    [Table("SHIPMENT_DETAILED_DATA")]
    public class ShipmentDetailedDataItem
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("SHIPMENT_ID")]
        public int shipmentId { get; set; }

        [Column("SENDER_NAME")]
        public string senderName { get; set; }

        [Column("SENDER_ADDRESS")]
        public string senderAddress { get; set; }

        [Column("RECEIVER_NAME")]
        public string receiverName { get; set; }

        [Column("RECEIVER_ADDRESS")]
        public string receiverAddress { get; set; }

        [Column("TRACKING_NUMBER")]
        public string trackingNumber { get; set; }

        [Column("PACKAGES_COUNT")]
        public short packagesCount { get; set; }
    }
}