using System;

namespace WorkflowModule.Models
{
    public class EventExecutor
    {
        public Guid UserId { get; set; }
        public string Hierarchy { get; set; }
        public string[] Roles { get; set; }
    }
}
