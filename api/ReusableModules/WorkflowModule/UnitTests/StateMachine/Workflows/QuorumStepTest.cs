using System;
using WorkflowModule.StateMachine.Workflows;
using Xunit;

namespace UnitTests.StateMachine.Workflows
{
    public class QuorumStepTest
    {
        [Fact]
        public void Should_set_consensus_and_assigned_users()
        {
            var firstUser = Guid.NewGuid();
            var secontUser = Guid.NewGuid();
            var thirdUser = Guid.NewGuid();
            var users = new Guid[] { firstUser, secontUser, thirdUser };
            var step = new QuorumStep(users, 2);

            Assert.Equal(StepState.InProgress, step.StepState);
            Assert.False(step.IsCompleted);
        }

        [Fact]
        public void StepState_is_Approved_when_majority_approved()
        {
            var firstUser = Guid.NewGuid();
            var secontUser = Guid.NewGuid();
            var thirdUser = Guid.NewGuid();
            var users = new Guid[] { firstUser, secontUser, thirdUser };
            var step = new QuorumStep(users, 2);

            step.Vote(firstUser, VotingOptions.Approve);
            step.Vote(secontUser, VotingOptions.Reject);
            step.Vote(thirdUser, VotingOptions.Approve);

            Assert.Equal(StepState.Approved, step.StepState);
            Assert.True(step.IsCompleted);
        }

        [Fact]
        public void StepState_is_InProgress_when_some_approved()
        {
            var firstUser = Guid.NewGuid();
            var secontUser = Guid.NewGuid();
            var thirdUser = Guid.NewGuid();
            var users = new Guid[] { firstUser, secontUser, thirdUser };
            var step = new QuorumStep(users, 2);

            step.Vote(firstUser, VotingOptions.Approve);
            step.Vote(secontUser, VotingOptions.Reject);

            Assert.Equal(StepState.InProgress, step.StepState);
            Assert.False(step.IsCompleted);
        }

        [Fact]
        public void StepState_is_Rejected_when_all_vote_reject()
        {
            var firstUser = Guid.NewGuid();
            var secontUser = Guid.NewGuid();
            var thirdUser = Guid.NewGuid();
            var users = new Guid[] { firstUser, secontUser, thirdUser };
            var step = new QuorumStep(users, 2);

            step.Vote(firstUser, VotingOptions.Reject);
            step.Vote(secontUser, VotingOptions.Reject);
            step.Vote(thirdUser, VotingOptions.Reject);

            Assert.Equal(StepState.Rejected, step.StepState);
            Assert.True(step.IsCompleted);
        }

        [Fact]
        public void StepState_is_Rejected_when_quorum_cannot_be_reached()
        {
            var firstUser = Guid.NewGuid();
            var secontUser = Guid.NewGuid();
            var thirdUser = Guid.NewGuid();
            var users = new Guid[] { firstUser, secontUser, thirdUser };
            var step = new QuorumStep(users, 2);

            step.Vote(firstUser, VotingOptions.Reject);
            step.Vote(secontUser, VotingOptions.Reject);

            Assert.Equal(StepState.Rejected, step.StepState);
            Assert.True(step.IsCompleted);
        }
    }
}
