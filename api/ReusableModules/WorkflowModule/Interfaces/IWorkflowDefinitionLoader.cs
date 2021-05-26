using System.Collections.Generic;
using WorkflowModule.Descriptors;

namespace WorkflowModule.Interfaces
{
    public interface IWorkflowDefinitionLoader
    {
        Dictionary<string, WorkflowDescriptor> LoadWorkflows();
    }
}
