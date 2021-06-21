namespace WorkflowModule.StateMachine.Workflows
{
    public class StepTransition
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public int NextStepId { get; set; }
    }
}
