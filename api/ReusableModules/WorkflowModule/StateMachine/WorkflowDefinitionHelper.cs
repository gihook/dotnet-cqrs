using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class WorkflowDefinitionHelper
    {
        public virtual bool EventIsAllowed(string eventName, StateInfo stateInfo, string workflowId)
        {
            return false;
        }
    }
}
