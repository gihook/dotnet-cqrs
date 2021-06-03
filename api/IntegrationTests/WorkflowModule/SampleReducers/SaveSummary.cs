using IntegrationTests.WorkflowModule.SampleAggregates;
using Newtonsoft.Json.Linq;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace IntegrationTests.WorkflowModule.SampleReducers
{
    public class SaveSummary : IEventReducer
    {
        public object Reduce(object currentStateData, EventPayload payload)
        {
            var stateData = currentStateData as SubmissionStateData;
            var summary = payload.Data["summary"];
            stateData.Summary = summary as JObject;

            return stateData;
        }
    }
}

