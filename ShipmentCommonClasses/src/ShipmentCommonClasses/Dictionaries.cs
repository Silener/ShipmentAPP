using ShipmentCommonClasses.Enums;

namespace ShipmentCommonClasses
{
    public static class Dictionaries
    {
        public static Dictionary<ShipmentStatus, string> shipmentStatuses = new Dictionary<ShipmentStatus, string>
        {
            { ShipmentStatus.ShipmentStatusOutForDelivery, "Out for delivery" },
            { ShipmentStatus.ShipmentStatusDelivered, "Delivered" },
            { ShipmentStatus.ShipmentStatusHeld, "Held" }
        };
    }
}