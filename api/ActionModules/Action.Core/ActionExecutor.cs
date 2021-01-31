using System.Linq;
using System.Threading.Tasks;
using Action.Interfaces;
using Action.Models;

namespace Action.Core
{
    public class ActionExecutor : IActionExecutor
    {
        public async Task<ActionResult> Execute(IAction action, Executor executor)
        {
            var isAuthorized = await action.IsAuthorized(executor);
            if (!isAuthorized) return ActionResult.Unauthorized();

            var errors = await action.Validate(executor);

            if (errors.Any()) return ActionResult.BadRequest(errors);

            var data = await action.Execute(executor);

            return ActionResult.OkRequest(data);
        }
    }
}
