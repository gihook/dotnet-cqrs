using System.Collections.Generic;

namespace WebAPI.ActionModels
{
    public class ActionDescription
    {
        public string ActionName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
