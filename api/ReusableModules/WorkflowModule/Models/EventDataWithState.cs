namespace WorkflowModule.Models
{
    public class EventDataWithState
    {
        public EventPayload EventPayload { get; set; }
        public StateInfo StateInfo { get; set; }
        public string WorkflowId { get; set; }
    }
}
