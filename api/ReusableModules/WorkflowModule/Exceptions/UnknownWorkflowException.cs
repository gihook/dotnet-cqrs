using System;

namespace WorkflowModule.Exceptions
{
    public class UnknownWorkflowException : Exception
    {
        public string WorkflowId { get; set; }

        public UnknownWorkflowException(string workflowId)
        {
            WorkflowId = workflowId;
        }
    }
}

