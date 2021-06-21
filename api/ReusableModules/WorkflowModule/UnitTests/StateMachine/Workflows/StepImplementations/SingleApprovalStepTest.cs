using System;
using WorkflowModule.StateMachine.Workflows;
using WorkflowModule.StateMachine.Workflows.StepImplementations;
using Xunit;

namespace UnitTests.StateMachine.Workflows.StepImplementations
{
    public class SingleApprovalStepTest
    {
        [Fact]
        public void StepActions_should_return_voting_actions()
        {
            var step = new SingleApprovalStep();

            var actions = step.StepActions;

            Assert.Collection(actions, action =>
            {
                Assert.Equal("vote-accept", action);
            }, action =>
            {
                Assert.Equal("vote-reject", action);
            });
        }

        [Fact]
        public void ExecuteAction_should_return_same_step_id_if_voting_is_not_done()
        {
            var step = new SingleApprovalStep();
            step.Id = "s1";

            var actions = step.StepActions;

            var firstUser = Guid.NewGuid();
            var secondUser = Guid.NewGuid();
            var thirdUser = Guid.NewGuid();

            step.AssignedUsers = new Guid[] { firstUser, secondUser, thirdUser };

            step.ExecuteAction("vote-reject", firstUser);
            var nextStepId = step.ExecuteAction("vote-reject", thirdUser);

            Assert.Equal("s1", nextStepId);
        }

        [Fact]
        public void ExecuteAction_should_return_approval_step_id_when_voting_is_done_and_accepted()
        {
            var step = new SingleApprovalStep();
            step.Id = "s1";

            var actions = step.StepActions;

            var firstUser = Guid.NewGuid();
            var secondUser = Guid.NewGuid();
            var thirdUser = Guid.NewGuid();

            step.AssignedUsers = new Guid[] { firstUser, secondUser, thirdUser };

            step.ApprovalTransition = new StepTransition()
            {
                Id = "t1",
                Label = "Go to S5",
                NextStepId = "s5",
            };

            step.ExecuteAction("vote-reject", firstUser);
            step.ExecuteAction("vote-reject", secondUser);
            var nextStepId = step.ExecuteAction("vote-accept", thirdUser);

            Assert.Equal("s5", nextStepId);
        }

        [Fact]
        public void ExecuteAction_should_move_to_rejection_step_id_when_voting_is_done_and_rejected()
        {
            var step = new SingleApprovalStep();
            step.Id = "s1";

            var actions = step.StepActions;

            var firstUser = Guid.NewGuid();
            var secondUser = Guid.NewGuid();
            var thirdUser = Guid.NewGuid();

            step.AssignedUsers = new Guid[] { firstUser, secondUser, thirdUser };

            step.RejectionTransition = new StepTransition()
            {
                Id = "t2",
                Label = "Go to S5",
                NextStepId = "s6",
            };

            step.ExecuteAction("vote-reject", firstUser);
            step.ExecuteAction("vote-reject", secondUser);
            var nextStepId = step.ExecuteAction("vote-reject", thirdUser);

            Assert.Equal("s6", nextStepId);
        }
    }
}
