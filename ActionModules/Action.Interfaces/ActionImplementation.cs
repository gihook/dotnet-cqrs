using System.Collections.Generic;
using Action.Models;

namespace Action.Interfaces
{
    public abstract class ActionImplementation<T> : IAction
    {
        protected abstract T ExecuteInternal(Executor executor);

        public object Execute(Executor executor)
        {
            return ExecuteInternal(executor);
        }

        public abstract IEnumerable<ValidationError> Validate(Executor executor);
    }
}
