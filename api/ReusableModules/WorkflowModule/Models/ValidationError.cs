namespace WorkflowModule.Models
{
    public class ValidationError
    {
        public string Id { get; set; }
        public string ParameterName { get; set; }
        public object[] Parameters { get; set; } = new object[0];

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

        public static ValidationError ValidatiorFunctionError(string type, object[] parameters)
        {
            var instance = new ValidationError();
            instance.Id = "ValidatiorFunctionError";
            instance.ParameterName = type;
            instance.Parameters = parameters;

            return instance;
        }
    }
}
