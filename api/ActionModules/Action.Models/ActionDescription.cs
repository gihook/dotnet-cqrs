using System.Collections.Generic;

namespace Action.Models
{
    public class ActionDescription
    {
        public string ActionName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
