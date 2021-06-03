using IntegrationTests.WorkflowModule.SampleAggregates;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace IntegrationTests.WorkflowModule.SampleReducers
{
    public class SaveSummary : IEventReducer
    {
        public object Reduce(object currentStateData, EventPayload payload)
        {
            var stateData = currentStateData as SubmissionStateData;
            stateData.Summary = new
            {
                General = new
                {
                    Fdoa = "test123"
                }
            };

            return stateData;
        }
    }
}

