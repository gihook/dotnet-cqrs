using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowModule.StateMachine.Workflows.StepImplementations
{
    public enum StepVote
    {
        Accepted,
        Rejected
    }

    public class ConsensusStep : Step
    {
        private Dictionary<Guid, StepVote> _votes = new Dictionary<Guid, StepVote>();

        public StepTransition ApprovalTransition { get; set; }
        public StepTransition RejectionTransition { get; set; }

        public override IEnumerable<string> StepActions
        {
            get
            {
                return new string[] { "vote-accept", "vote-reject" };
            }
        }

        public override string ExecuteAction(string actionName, Guid userId)
        {
            if (actionName == "vote-accept") _votes[userId] = StepVote.Accepted;
            if (actionName == "vote-reject") _votes[userId] = StepVote.Rejected;

            var hasRejectedVote = _votes.Any(kvp => kvp.Value == StepVote.Rejected);

            if (hasRejectedVote) return RejectionTransition.NextStepId;

            var numberOfAcceptedVotes = _votes.Count(kvp => kvp.Value == StepVote.Accepted);
            var numberOfVoters = AssignedUsers.Count();

            if (numberOfVoters == numberOfAcceptedVotes)
            {
                return ApprovalTransition.NextStepId;
            }

            return Id;
        }
    }
}
