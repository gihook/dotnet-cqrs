using System;
using Newtonsoft.Json.Linq;

namespace IntegrationTests.WorkflowModule.SampleAggregates
{
    public class SubmissionStateData
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public JObject Summary { get; set; }

        public Guid CreatorUserId { get; set; }
    }
}
