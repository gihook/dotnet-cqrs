using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowModule.StateMachine.Workflows
{
    public class QuorumStep : VotingStep
    {
        private int _majorityCount;

        public QuorumStep(IEnumerable<Guid> users, int majorityCount)
        {
            AssignedUsers = users;
            _majorityCount = majorityCount;
        }

        public override StepState StepState
        {
            get
            {
                var numberOfAssignedUsers = AssignedUsers.Count();

                var groupedVotes = Votes.Keys.GroupBy(key => GetUserVote(key)).ToDictionary(x => x.Key, x => x.Count());

                var numberOfRejectedVotes = groupedVotes.ContainsKey(VotingOptions.Reject)
                                                ? groupedVotes[VotingOptions.Reject]
                                                : 0;
                var isRejected = numberOfRejectedVotes > (numberOfAssignedUsers - _majorityCount);

                if (isRejected) return StepState.Rejected;

                var numberOfApprovedVotes = groupedVotes.ContainsKey(VotingOptions.Approve)
                                                ? groupedVotes[VotingOptions.Approve]
                                                : 0;

                var isApproved = numberOfApprovedVotes >= _majorityCount;

                return isApproved ? StepState.Approved : StepState.InProgress;
            }
        }
    }
}
