using WebAPI.ActionInterfaces;
using WebAPI.ActionModels;

namespace WebAPI.ActionCore
{
    public class ActionExecutor : IActionExecutor
    {
        public ActionResult<T> Execute<T>(IAction<T> action, Executor executor)
        {
            var errors = action.Validate(executor);
            var data = action.Execute(executor);

            var actionResult = new ActionResult<T>
            {
                ResultStatus = ActionResultStatus.Ok,
                ResultData = data
            };

            return actionResult;
        }
    }
}
