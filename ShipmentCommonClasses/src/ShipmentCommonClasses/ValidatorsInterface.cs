namespace ShipmentCommonClasses
{
    public abstract class IValidator<T>
    {
        public abstract List<string> Validate(T validationObject);

        public List<string> validationMessages = new List<string>();
    }

}