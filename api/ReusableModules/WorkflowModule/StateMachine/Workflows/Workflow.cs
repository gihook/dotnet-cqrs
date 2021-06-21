using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowModule.StateMachine.Workflows.StepImplementations;

namespace WorkflowModule.StateMachine.Workflows
{
    public class Workflow
    {
        public Step CurrentStep { get; set; }
        public Dictionary<string, Step> Steps { get; set; } = new Dictionary<string, Step>();

        public bool IsCompleted => CurrentStep.GetType() == typeof(FinalStep);

        public Workflow(IEnumerable<Step> steps)
        {
            Steps = steps.ToDictionary(s => s.Id, s => s);
            CurrentStep = Steps[Step.ORIGINATOR_STEP_ID];
        }

        public void ExecuteAction(string actionName, Guid userId)
        {
            var nextStepId = CurrentStep.ExecuteAction(actionName, userId);
            CurrentStep = Steps[nextStepId];
        }
    }
}
