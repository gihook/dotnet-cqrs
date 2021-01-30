using System.Threading.Tasks;
using Action.Models;

namespace Action.Interfaces
{
    public interface IActionExecutor
    {
        Task<ActionResult> Execute(IAction action, Executor executor);
    }
}
