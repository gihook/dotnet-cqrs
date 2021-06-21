using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowModule.StateMachine.Workflows
{
    public abstract class Step
    {
        public const string ORIGINATOR_STEP_ID = "originator-step";
        public const string ACCEPTED_STEP_ID = "accepted-step";
        public const string REJECTED_STEP_ID = "rejected-step";

        public string Id { get; set; }
        public string Label { get; set; }
        public string StatusLabel { get; set; }
        public IEnumerable<Guid> AssignedUsers { get; set; }

        public virtual IEnumerable<string> StepActions { get; } = Enumerable.Empty<string>();

        public abstract string ExecuteAction(string actionName, Guid userId);
    }
}
