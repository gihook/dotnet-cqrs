using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowModule.StateMachine.Workflows
{
    public class ConsensusStep : VotingStep
    {
        public override StepState StepState
        {
            get
            {
                var numberOfVotes = Votes.Count();
                var numberOfAssignedUsers = AssignedUsers.Count();

                var isCompleted = numberOfAssignedUsers == numberOfVotes;

                if (!isCompleted) return StepState.InProgress;

                var hasConsensus = AssignedUsers.All(u =>
                {
                    return GetUserVote(u) == VotingOptions.Approve;
                });

                if (hasConsensus) return StepState.Approved;

                return StepState.Rejected;
            }
        }

        public ConsensusStep(IEnumerable<Guid> users)
        {
            AssignedUsers = users;
        }
    }
}
