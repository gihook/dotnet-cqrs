namespace Action.Models
{
    public class ValidationError
    {
        public string FieldName { get; set; }
        public object Value { get; set; }
        public string ErrorCode { get; set; }
    }
}
