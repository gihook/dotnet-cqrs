
using Action.Models;

namespace Action.Interfaces
{
    public interface IActionExecutor
    {
        ActionResult Execute(IAction action, Executor executor);
    }
}
