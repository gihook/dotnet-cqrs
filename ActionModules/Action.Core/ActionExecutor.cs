using Action.Interfaces;
using Action.Models;

namespace Action.Core
{
    public class ActionExecutor : IActionExecutor
    {
        public ActionResult Execute(IAction action, Executor executor)
        {
            var errors = action.Validate(executor);
            var data = action.Execute(executor);

            var actionResult = new ActionResult
            {
                ResultStatus = ActionResultStatus.Ok,
                ResultData = data
            };

            return actionResult;
        }
    }
}
