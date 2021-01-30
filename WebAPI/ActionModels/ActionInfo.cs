using System.Collections.Generic;

namespace WebAPI.ActionModels
{
    public class ActionInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string FullName { get; set; }
        public IEnumerable<ActionParameter> Parameters { get; set; }
    }
}
