namespace WorkflowModule.Models
{
    public class ValidationError
    {
        public string Id { get; set; }
        public string ParameterName { get; set; }

        public static ValidationError OrderNumberConfilict()
        {
            var instance = new ValidationError();
            instance.Id = "OrderNumberConfilict";

            return instance;
        }

        public static ValidationError EventNotAllowed()
        {
            var instance = new ValidationError();
            instance.Id = "EventNotAllowed";

            return instance;
        }

        public static ValidationError RequiredInputParameter(string paramName)
        {
            var instance = new ValidationError();
            instance.Id = "RequiredInputParameterNotProvided";
            instance.ParameterName = paramName;

            return instance;
        }

        public static ValidationError TypeParameterError(string paramName)
        {
            var instance = new ValidationError();
            instance.Id = "InvalidInputType";
            instance.ParameterName = paramName;

            return instance;
        }

        public static ValidationError ValidatiorFunctionError(string type)
        {
            var instance = new ValidationError();
            instance.Id = "ValidatiorFunctionError";
            instance.ParameterName = type;

            return instance;
        }
    }
}
