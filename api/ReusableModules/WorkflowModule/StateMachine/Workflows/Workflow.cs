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

        public static bool AreValidSteps(IEnumerable<Step> steps)
        {
            var hasOriginatorStep = steps.Any(s => s.Id == Step.ORIGINATOR_STEP_ID);
            if (!hasOriginatorStep) return false;

            var hasAcceptedStep = steps.Any(s => s.Id == Step.ACCEPTED_STEP_ID);
            var hasRejectedStep = steps.Any(s => s.Id == Step.REJECTED_STEP_ID);

            if (!hasAcceptedStep || !hasRejectedStep) return false;

            return true;
        }
    }
}
