using System.Collections.Generic;

namespace Action.Models
{
    public class ActionParameter
    {
        public string ParameterName { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public IEnumerable<string> Generics { get; set; }
        public string ComponentName { get; set; }
        public IEnumerable<ValidatorDescriptor> Validators { get; set; }
    }
}
