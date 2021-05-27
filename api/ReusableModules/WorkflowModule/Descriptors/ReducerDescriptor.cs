using System.Collections.Generic;
using System.Linq;

namespace WorkflowModule.Descriptors
{
    public class ReducerDescriptor
    {
        public string Type { get; set; }
        public IEnumerable<object> Params { get; set; } = Enumerable.Empty<object>();
    }
}
