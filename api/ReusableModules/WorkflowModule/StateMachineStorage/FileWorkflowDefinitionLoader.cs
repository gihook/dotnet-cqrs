using System.Collections.Generic;
using System.IO;
using WorkflowModule.Descriptors;
using WorkflowModule.Interfaces;

namespace WorkflowModule.StateMachineStorage
{
    public class FileWorkflowDefinitionLoader : IWorkflowDefinitionLoader
    {
        private readonly string _folderContainingWorkflowSpecifications;

        public FileWorkflowDefinitionLoader(string folderContainingWorkflowSpecifications)
        {
            _folderContainingWorkflowSpecifications = folderContainingWorkflowSpecifications;
        }

        public Dictionary<string, WorkflowDescriptor> LoadWorkflows()
        {
            var descriptors = new Dictionary<string, WorkflowDescriptor>();
            var files = Directory.EnumerateFiles(_folderContainingWorkflowSpecifications);
            var parser = new StateMachineParser();

            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                var workflowDefinition = parser.GetWorkflowDescriptor(content);
                descriptors[workflowDefinition.Id] = workflowDefinition;
            }

            return descriptors;
        }
    }
}
