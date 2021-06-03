using IntegrationTests.WorkflowModule.SampleAggregates;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace IntegrationTests.WorkflowModule.SampleReducers
{
    public class InitializeSubmission : IEventReducer
    {
        public object Reduce(object currentStateData, EventPayload payload)
        {
            var stateData = new SubmissionStateData();
            stateData.Title = payload.Data["title"].ToString();
            stateData.Description = payload.Data["description"] != null ? payload.Data["description"].ToString() : "";
            stateData.CreatorUserId = payload.EventExecutor.UserId;

            return stateData;
        }
    }
}
