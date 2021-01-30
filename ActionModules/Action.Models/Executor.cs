using System.Collections.Generic;

namespace Action.Models
{
    public class Executor
    {
        public string Id { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
