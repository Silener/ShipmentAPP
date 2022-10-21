using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using ShipmentCommonClasses.Enums;

namespace ShipmentCommonClasses.Dtos
{
    [Keyless]
    public class ShipmentListData
    {
        public int shipmentId { get; set; }

        public int truckId { get; set; }

        public ShipmentStatus shipmentStatus { get; set; }

        [DisplayName("Receiver Name")]
        public string receiverName { get; set; }

        [DisplayName("Receiver Address")]
        public string receiverAddress { get; set; }

        [DisplayName("Packages Count")]
        public short packagesCount { get; set; }
    }
}