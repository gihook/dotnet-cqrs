using System;

namespace WorkflowModule.StateMachine.Workflows.StepImplementations
{
    public class FinalStep : Step
    {
        private FinalStep() { }

        public static Step Accepted()
        {
            var step = new FinalStep();
            step.Id = Step.ACCEPTED_STEP_ID;
            step.StepStatus = StepStatus.Accepted;

            return step;
        }

        public static Step Rejected()
        {
            var step = new FinalStep();
            step.Id = Step.REJECTED_STEP_ID;
            step.StepStatus = StepStatus.Rejected;

            return step;
        }

        public override string ExecuteAction(string actionName, Guid userId)
        {
            return "";
        }
    }
}


