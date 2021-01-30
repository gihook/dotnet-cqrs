using System.Collections.Generic;
using System.Threading.Tasks;
using Action.Models;

namespace Action.Interfaces
{
    public abstract class ActionImplementation<T> : IAction
    {
        protected abstract Task<T> ExecuteInternal(Executor executor);

        public virtual async Task<bool> IsAuthorized(Executor executor)
        {
            await Task.CompletedTask;
            return true;
        }

        public async Task<object> Execute(Executor executor)
        {
            return await ExecuteInternal(executor);
        }

        public abstract Task<IEnumerable<ValidationError>> Validate(Executor executor);
    }
}
