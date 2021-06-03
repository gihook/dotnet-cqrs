using System;

namespace IntegrationTests.WorkflowModule.SampleAggregates
{
    public class SubmissionStateData
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public object Summary { get; set; }

        public Guid CreatorUserId { get; set; }
    }
}
