using System.Linq;

namespace WorkflowModule.StateMachine.Workflows.StepImplementations
{
    public class SingleApprovalStep : VotingStep
    {

        protected override bool IsAccepted()
        {
            return _votes.Any(kvp => kvp.Value == StepVote.Accepted);
        }

        protected override bool IsRejected()
        {
            var numberOfRejectedVotes = _votes.Count(kvp => kvp.Value == StepVote.Rejected);
            var numberOfVoters = AssignedUsers.Count();

            return numberOfVoters == numberOfRejectedVotes;
        }
    }
}
