using System.IO;
using System.Reflection;
using WorkflowModule.StateMachineStorage;
using Xunit;

namespace UnitTests.WorkflowStorage
{
    public class FileWorkflowDefinitionLoaderTest
    {
        [Fact]
        public void Should_Load_Files_From_FS()
        {
            var assemblyLocation = Assembly.GetEntryAssembly().Location;
            var assemblyFolder = Path.GetDirectoryName(assemblyLocation);

            var testFilesFolder = Path.Combine(assemblyFolder, "../../../UnitTests/ExampleFiles");

            var definitionLoader = new FileWorkflowDefinitionLoader(testFilesFolder);

            var workflowDefinitions = definitionLoader.LoadWorkflows();

            Assert.Single(workflowDefinitions);

            var sampleWorkflow = workflowDefinitions["SubmissionsWorkflow"];
            Assert.Equal("SubmissionsWorkflow", sampleWorkflow.Id);
        }
    }
}
