using System.ComponentModel.DataAnnotations;

namespace ShipmentCommonClasses.Enums
{
    public enum CustomerShipmentAcceptanceStatus : short
    {
        [Display(Name = "Select Value")]
        StatusUndefined,
        [Display(Name = "Customer Was At Home")]
        StatusCustomerWasAtHome,
        [Display(Name = "Customer Was Not At Home")]
        StatusCustomerWasNotAtHome
    }

    public enum PackageIntegrity : short
    {
        [Display(Name = "Select Value")]
        IntegrityUndefined,
        [Display(Name = "Package Was Damaged")]
        IntegrityDamaged,
        [Display(Name = "Package Was Undamaged")]
        IntegrityNotDamaged
    }

    public enum ShipmentStatus : short
    {
        [Display(Name = "All")]
        ShipmentStatusUndefined,

        [Display(Name = "Out for delivery")]
        ShipmentStatusOutForDelivery,

        [Display(Name = "Delivered")]
        ShipmentStatusDelivered,

        [Display(Name = "Held-In-Truck")]
        ShipmentStatusHeld
    }
}