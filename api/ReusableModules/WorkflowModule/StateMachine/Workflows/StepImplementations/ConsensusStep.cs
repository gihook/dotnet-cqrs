using System;
using System.Linq;

namespace WorkflowModule.StateMachine.Workflows.StepImplementations
{
    public class ConsensusStep : VotingStep
    {
        protected override bool IsAccepted()
        {
            var numberOfAcceptedVotes = _votes.Count(kvp => kvp.Value == StepVote.Accepted);
            var numberOfVoters = AssignedUsers.Count();

            return numberOfVoters == numberOfAcceptedVotes;
        }

        protected override bool IsRejected()
        {
            return _votes.Any(kvp => kvp.Value == StepVote.Rejected);
        }
    }
}
