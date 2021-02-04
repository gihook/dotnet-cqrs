using System.Collections.Generic;

namespace Action.Models
{
    public class ActionInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FormDisplayName { get; set; }
        public string Type { get; set; } // enum
        public string FullName { get; set; }
        public IEnumerable<ActionParameter> Parameters { get; set; }
        public string ReturnType { get; set; }
    }
}
