using System.Collections.Generic;
using System.Linq;

namespace WorkflowModule.StateMachine.Workflows
{
    public class WorkflowInstance
    {
        private WorkflowDecision _decision = WorkflowDecision.None;

        public int CurrentStepIndex => Steps.Count(s => s.IsCompleted);

        public Step CurrentStep => Steps.ElementAtOrDefault(CurrentStepIndex);

        public IEnumerable<Step> Steps { get; set; }

        public WorkflowDecision Decision
        {
            get
            {
                var stepCount = Steps.Count();
                var workflowDone = CurrentStepIndex == stepCount;

                if (workflowDone)
                {
                    var lastStep = Steps.ElementAt(CurrentStepIndex - 1);

                    return (WorkflowDecision)lastStep.StepState;
                }

                return _decision;
            }
        }

        public IEnumerable<string> AvailableActions
        {
            get
            {
                return CurrentStep.AvailableActions;
            }
        }

        public void ExecuteAction(string actionName, object[] parameters)
        {
            var step = CurrentStep;
            var type = step.GetType();
            var methodInfo = type.GetMethod(actionName);
            methodInfo.Invoke(step, parameters);
        }

        public void ReturnToOriginator()
        {
            foreach (var step in Steps)
            {
                step.Reset();
            }
        }
    }
}
