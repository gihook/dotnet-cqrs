using System;

namespace WorkflowModule.StateMachine.Workflows
{
    public class SingleUserStep : Step
    {
        private StepState _stepState = StepState.InProgress;

        public override StepState StepState => _stepState;

        public SingleUserStep(Guid user)
        {
            AssignedUsers = new Guid[] { user };
            _stepState = StepState.InProgress;
        }

        public void Approve()
        {
            _stepState = StepState.Approved;
        }

        public void Reset()
        {
            _stepState = StepState.InProgress;
        }
    }
}
