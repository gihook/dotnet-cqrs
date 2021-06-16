using System;
using WorkflowModule.StateMachine.Workflows;
using Xunit;

namespace UnitTests.StateMachine.Workflows
{
    public class ConsensusStepTest
    {
        [Fact]
        public void Should_assign_users_when_create()
        {
            var firstUserId = Guid.NewGuid();
            var users = new Guid[] { firstUserId };
            var step = new ConsensusStep(users);
            step.Id = "test-id";

            Assert.Collection(step.AssignedUsers, u =>
            {
                Assert.Equal(firstUserId, u);
            });

            Assert.Equal("ConsensusStep", step.StepType);
            Assert.Equal("test-id", step.Id);
        }

        [Fact]
        public void Should_mark_as_approved_when_all_users_approve()
        {
            var firstUserId = Guid.NewGuid();
            var secondUserId = Guid.NewGuid();
            var users = new Guid[] { firstUserId, secondUserId };

            var step = new ConsensusStep(users);

            step.Vote(firstUserId, VotingOptions.Approve);
            step.Vote(secondUserId, VotingOptions.Approve);

            Assert.Equal(StepState.Approved, step.StepState);
        }

        [Fact]
        public void Should_mark_as_rejected_when_one_user_rejected()
        {
            var firstUserId = Guid.NewGuid();
            var secondUserId = Guid.NewGuid();
            var users = new Guid[] { firstUserId, secondUserId };

            var step = new ConsensusStep(users);

            step.Vote(firstUserId, VotingOptions.Approve);
            step.Vote(secondUserId, VotingOptions.Reject);

            Assert.Equal(StepState.Rejected, step.StepState);
        }

        [Fact]
        public void Should_mark_as_InProgress_when_not_all_user_voted()
        {
            var firstUserId = Guid.NewGuid();
            var secondUserId = Guid.NewGuid();
            var users = new Guid[] { firstUserId, secondUserId };

            var step = new ConsensusStep(users);

            step.Vote(firstUserId, VotingOptions.Approve);

            Assert.Equal(StepState.InProgress, step.StepState);
        }
    }
}
