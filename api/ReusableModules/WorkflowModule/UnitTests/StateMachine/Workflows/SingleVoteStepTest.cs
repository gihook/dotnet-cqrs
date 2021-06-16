using System;
using WorkflowModule.StateMachine.Workflows;
using Xunit;

namespace UnitTests.StateMachine.Workflows
{
    public class SingleVoteStepTest
    {

        [Fact]
        public void Should_create_SingleVoteStep()
        {
            var user = new Guid();
            var step = new SingleVoteStep(new Guid[] { user });

            Assert.Equal("SingleVoteStep", step.StepType);
            Assert.Collection(step.AssignedUsers, u =>
            {
                Assert.Equal(user, u);
            });
            Assert.Equal(StepState.InProgress, step.StepState);
            Assert.False(step.IsCompleted);
        }

        [Fact]
        public void Should_be_marked_as_approved_when_one_user_approves()
        {
            var firstUser = Guid.NewGuid();
            var secondUser = Guid.NewGuid();
            var step = new SingleVoteStep(new Guid[] { firstUser, secondUser });

            step.Vote(firstUser, VotingOptions.Reject);
            step.Vote(secondUser, VotingOptions.Approve);

            Assert.Equal(StepState.Approved, step.StepState);
            Assert.True(step.IsCompleted);
        }

        [Fact]
        public void Should_be_marked_as_rejected_when_everyone_rejects()
        {
            var firstUser = Guid.NewGuid();
            var secondUser = Guid.NewGuid();
            var step = new SingleVoteStep(new Guid[] { firstUser, secondUser });

            step.Vote(firstUser, VotingOptions.Reject);
            step.Vote(secondUser, VotingOptions.Reject);

            Assert.Equal(StepState.Rejected, step.StepState);
            Assert.True(step.IsCompleted);
        }

        [Fact]
        public void Should_be_marked_as_InProgress_when_only_some_reject()
        {
            var firstUser = Guid.NewGuid();
            var secondUser = Guid.NewGuid();
            var step = new SingleVoteStep(new Guid[] { firstUser, secondUser });

            step.Vote(secondUser, VotingOptions.Reject);

            Assert.Equal(StepState.InProgress, step.StepState);
            Assert.False(step.IsCompleted);
        }

        [Fact]
        public void Should_be_marked_as_Approved_when_only_one_user_accepts()
        {
            var firstUser = Guid.NewGuid();
            var secondUser = Guid.NewGuid();
            var step = new SingleVoteStep(new Guid[] { firstUser, secondUser });

            step.Vote(secondUser, VotingOptions.Approve);

            Assert.Equal(StepState.Approved, step.StepState);
            Assert.True(step.IsCompleted);
        }
    }
}
