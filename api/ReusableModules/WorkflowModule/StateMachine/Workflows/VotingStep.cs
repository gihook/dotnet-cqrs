using System;
using System.Collections.Generic;

namespace WorkflowModule.StateMachine.Workflows
{
    public abstract class VotingStep : Step
    {
        public Dictionary<Guid, VotingOptions> Votes { get; set; } = new Dictionary<Guid, VotingOptions>();

        public void Vote(Guid userId, VotingOptions vote)
        {
            Votes[userId] = vote;
        }

        protected VotingOptions GetUserVote(Guid userId)
        {
            return Votes.ContainsKey(userId) ? Votes[userId] : VotingOptions.None;
        }

        public override void Reset()
        {
            Votes = new Dictionary<Guid, VotingOptions>();
        }
    }
}
