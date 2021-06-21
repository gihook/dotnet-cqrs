using System;

namespace WorkflowModule.StateMachine.Workflows.StepImplementations
{
    public class SingleUserStep : Step
    {
        public override string ExecuteAction(string actionName, Guid userId)
        {
            return "";
        }
    }
}
