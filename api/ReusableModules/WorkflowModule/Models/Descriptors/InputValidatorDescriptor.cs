using System.Collections.Generic;

namespace WorkflowModule.Descriptors
{
    public class InputValidatorDescriptor
    {
        public string Type { get; set; }
        public IEnumerable<string> Params { get; set; }
    }
}
