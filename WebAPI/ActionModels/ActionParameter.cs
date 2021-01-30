using System.Collections.Generic;

namespace WebAPI.ActionModels
{
    public class ActionParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public IEnumerable<string> Generics { get; set; }
    }
}
