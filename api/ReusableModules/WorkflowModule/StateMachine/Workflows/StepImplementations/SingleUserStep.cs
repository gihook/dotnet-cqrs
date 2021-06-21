using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowModule.StateMachine.Workflows.StepImplementations
{
    public class SingleUserStep : Step
    {
        public IEnumerable<StepTransition> Transitions { get; set; }

        public override IEnumerable<string> StepActions
        {
            get
            {
                return Transitions.Select(t => t.Id);
            }
        }

        public override string ExecuteAction(string actionName, Guid userId)
        {
            var transition = Transitions.First(t => t.Id == actionName);

            return transition.NextStepId;
        }
    }
}
