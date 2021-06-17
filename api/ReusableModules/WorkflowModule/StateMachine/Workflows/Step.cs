using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WorkflowModule.StateMachine.Workflows
{
    public abstract class Step
    {
        private string[] BUILT_IN_METHODS = { "GetType", "ToString", "Equals", "GetHashCode" };

        public string Id { get; set; }

        public string Label { get; set; }

        public string StepType => this.GetType().Name;

        public IEnumerable<Guid> AssignedUsers { get; set; }

        public abstract StepState StepState { get; }

        public virtual bool IsCompleted
        {
            get
            {
                return StepState != StepState.InProgress;
            }
        }

        public IEnumerable<string> AvailableActions
        {
            get
            {
                var type = this.GetType();
                var methodInfos = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);

                return methodInfos
                        .Select(x => x.Name)
                        .Where(name =>
                        {
                            var isProperty = name.StartsWith("get_") || name.StartsWith("set_");
                            var isResetMethod = name == "Reset";
                            var isBuiltInMethod = BUILT_IN_METHODS.Any(m => m == name);

                            return !isProperty && !isResetMethod && !isBuiltInMethod;
                        });
            }
        }

        public abstract void Reset();
    }
}
