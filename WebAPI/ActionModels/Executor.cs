using System.Collections.Generic;

namespace WebAPI.ActionModels
{
    public class Executor
    {
        public string Id { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
