
using WebAPI.ActionModels;

namespace WebAPI.ActionInterfaces
{
    public interface IActionExecutor
    {
        ActionResult<T> Execute<T>(IAction<T> action, Executor executor);
    }
}
