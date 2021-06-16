using System;
using System.Collections.Generic;

namespace WorkflowModule.StateMachine.Workflows
{
    public abstract class Step
    {
        public string Id { get; set; }

        public string StepType => this.GetType().Name;

        public IEnumerable<Guid> AssignedUsers { get; set; }

        public abstract StepState StepState { get; }

        public bool IsCompleted
        {
            get
            {
                return StepState != StepState.InProgress;
            }
        }
    }
}
