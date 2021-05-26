using System.Collections.Generic;

namespace WorkflowModule.Descriptors
{
    public class ConditionalTransition
    {
        public string Condition { get; set; }
        public IEnumerable<string> Params { get; set; }
    }
}
