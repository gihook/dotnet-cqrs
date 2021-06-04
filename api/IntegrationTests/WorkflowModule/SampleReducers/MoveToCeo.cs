using System;
using IntegrationTests.WorkflowModule.SampleAggregates;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace IntegrationTests.WorkflowModule.SampleReducers
{
    public class MoveToCeo : IEventReducer
    {
        public object Reduce(object currentStateData, EventPayload payload)
        {
            var stateData = currentStateData as SubmissionStateData;
            var reviewerIdAsString = payload.Data["ceoUserId"].ToString();
            stateData.CeoReviewer = Guid.Parse(reviewerIdAsString);

            return stateData;
        }
    }
}
