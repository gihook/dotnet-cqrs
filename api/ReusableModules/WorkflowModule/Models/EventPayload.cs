using System;
using Newtonsoft.Json.Linq;

namespace WorkflowModule.Models
{
    public class EventPayload
    {
        public int OrderNumber { get; set; }
        public Guid AggregateId { get; set; }
        public string EventName { get; set; }
        public JObject Data { get; set; }
        public EventExecutor eventExecutor { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
