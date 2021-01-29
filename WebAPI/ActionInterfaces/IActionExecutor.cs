
using WebAPI.ActionModels;

namespace WebAPI.ActionInterfaces
{
    public interface IActionExecutor
    {
        ActionResult Execute(IAction action, Executor executor);
    }
}
