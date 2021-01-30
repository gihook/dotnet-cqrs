using System.Collections.Generic;
using WebAPI.ActionModels;

namespace WebAPI.ActionInterfaces
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
