using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowModule.StateMachine.Workflows
{
    public class SingleVoteStep : VotingStep
    {
        public override StepState StepState
        {
            get
            {
                var isRejected = AssignedUsers.All(u =>
                {
                    return GetUserVote(u) == VotingOptions.Reject;
                });

                if (isRejected) return StepState.Rejected;

                var isApproved = AssignedUsers.Any(u =>
                {
                    return GetUserVote(u) == VotingOptions.Approve;
                });

                return isApproved ? StepState.Approved : StepState.InProgress;
            }
        }

        public SingleVoteStep(IEnumerable<Guid> users)
        {
            AssignedUsers = users;
        }
    }
}
