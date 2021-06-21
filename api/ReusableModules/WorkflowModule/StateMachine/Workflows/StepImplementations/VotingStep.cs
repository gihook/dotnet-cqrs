using System;
using System.Collections.Generic;

namespace WorkflowModule.StateMachine.Workflows.StepImplementations
{
    public enum StepVote
    {
        Accepted,
        Rejected
    }

    public abstract class VotingStep : Step
    {
        protected Dictionary<Guid, StepVote> _votes = new Dictionary<Guid, StepVote>();

        public StepTransition ApprovalTransition { get; set; }
        public StepTransition RejectionTransition { get; set; }

        public void HandleVoteAction(string actionName, Guid userId)
        {
            if (actionName == "vote-accept") _votes[userId] = StepVote.Accepted;
            if (actionName == "vote-reject") _votes[userId] = StepVote.Rejected;
        }

        public override IEnumerable<string> StepActions
        {
            get
            {
                return new string[] { "vote-accept", "vote-reject" };
            }
        }

        protected abstract bool IsAccepted();
        protected abstract bool IsRejected();

        public override string ExecuteAction(string actionName, Guid userId)
        {
            HandleVoteAction(actionName, userId);

            if (IsAccepted()) return ApprovalTransition.NextStepId;
            if (IsRejected()) return RejectionTransition.NextStepId;

            return Id;
        }
    }
}
