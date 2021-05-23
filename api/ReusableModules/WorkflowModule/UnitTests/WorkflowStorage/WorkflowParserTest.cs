using WorkflowModule.WorkflowStorage;
using Xunit;

namespace UnitTests.WorkflowStorage
{
    public class WorkflowParserTest
    {
        [Fact]
        public void ShouldParseWorkflowSpecificationFile()
        {
            var yamlContent = @"
id: SubmissionsWorkflow
name: Submission FSM
description: Imaginary submission for presenting FSM definition

version: 1
";
            var workflowParser = new WorkflowParser();
            var workflowDescriptor = workflowParser.GetWorkflowDescriptor(yamlContent);

            Assert.Equal("SubmissionsWorkflow", workflowDescriptor.Id);
            Assert.Equal("Submission FSM", workflowDescriptor.Name);
            Assert.Equal("Imaginary submission for presenting FSM definition", workflowDescriptor.Description);
            Assert.Equal(1, workflowDescriptor.Version);
        }
    }
}
