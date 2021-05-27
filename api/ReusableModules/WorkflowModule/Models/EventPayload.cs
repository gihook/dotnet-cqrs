using System;

namespace WorkflowModule.Models
{
    public class EventPayload
    {
        public int OrderNumber { get; set; }
        public Guid AggregateId { get; set; }
        public string EventName { get; set; }
        public object Data { get; set; }
        public EventExecutor eventExecutor { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
