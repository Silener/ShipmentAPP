using System.Reflection;
using Shipment.ViewModels;
using ShipmentCommonClasses;
using ShipmentCommonClasses.Enums;

namespace Shipment.Validators
{
    public class DeliveryFinalizeValidators : IValidator<ShipmentDetailsViewModel>
    {
        public override List<string> Validate(ShipmentDetailsViewModel validationObject)
        {
            validationObject.validationMessages = new List<string>();
            switch (validationObject.shipmentDetails.shipmentStatus)
            {
                case ShipmentStatus.ShipmentStatusDelivered:
                    {
                        ValidateDelivered(validationObject);
                        break;
                    }

                case ShipmentStatus.ShipmentStatusHeld:
                    {
                        ValidateHeld(validationObject);
                        break;
                    }

            }

            return validationMessages;
        }

        private void ValidateDelivered(ShipmentDetailsViewModel shipmentDetailsViewModel)
        {
            if (shipmentDetailsViewModel.shipmentDetails.customerAcceptance == CustomerShipmentAcceptanceStatus.StatusUndefined)
                validationMessages.Add("You must select Shipment Acceptance in order to mark the shipment as delivered.");

            if (shipmentDetailsViewModel.shipmentDetails.packageIntegrity == PackageIntegrity.IntegrityUndefined)
                validationMessages.Add("You must select Package Integrity in order to mark the shipment as delivered.");

        }

        private void ValidateHeld(ShipmentDetailsViewModel shipmentDetailsViewModel)
        {
            if (shipmentDetailsViewModel.shipmentDetails.shipmentAdditionalNotes == null
             || shipmentDetailsViewModel.shipmentDetails.shipmentAdditionalNotes.Length <= 0)
                validationMessages.Add("You must enter Additional Notes in order to mark the shipment as held.");
        }

    }
}