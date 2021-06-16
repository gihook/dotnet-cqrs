using System;
using WorkflowModule.StateMachine.Workflows;
using Xunit;

namespace UnitTests.StateMachine.Workflows
{
    public class SingleUserStepTest
    {
        [Fact]
        public void Should_assign_user_when_step_is_created()
        {
            var userId = new Guid();
            var step = new SingleUserStep(userId);

            Assert.Collection(step.AssignedUsers, u =>
            {
                Assert.Equal(userId, u);
            });

            Assert.Equal(StepState.InProgress, step.StepState);
            Assert.False(step.IsCompleted);
        }

        [Fact]
        public void Should_complete_step_when_user_approves()
        {
            var userId = new Guid();
            var step = new SingleUserStep(userId);

            step.GoToNextStep();

            Assert.Equal(StepState.Approved, step.StepState);
            Assert.True(step.IsCompleted);
        }

        [Fact]
        public void Reset_should_move_to_Inactive_state()
        {
            var userId = new Guid();
            var step = new SingleUserStep(userId);

            step.Reset();
            Assert.Equal(StepState.InProgress, step.StepState);
            Assert.False(step.IsCompleted);
        }
    }
}
