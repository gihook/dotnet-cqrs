using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action.Models;

namespace Action.Interfaces
{
    public abstract class ActionImplementation<T> : IAction
    {
        private const string ADMIN_SCOPE = "Admin";

        protected abstract Task<T> ExecuteInternal(Executor executor);

        public virtual async Task<bool> IsAuthorized(Executor executor)
        {
            await Task.CompletedTask;
            return executor.Scopes.Any(s =>
            {
                var isAdmin = s == ADMIN_SCOPE;
                var actionFullName = this.GetType().FullName;
                var matchesAction = actionFullName.Contains(s);

                return isAdmin || matchesAction;
            });
        }

        public async Task<object> Execute(Executor executor)
        {
            return await ExecuteInternal(executor);
        }

        public abstract Task<IEnumerable<ValidationError>> Validate(Executor executor);
    }
}
