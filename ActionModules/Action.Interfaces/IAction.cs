using System.Collections.Generic;
using System.Threading.Tasks;
using Action.Models;

namespace Action.Interfaces
{
    public interface IAction
    {
        Task<bool> IsAuthorized(Executor executor);
        Task<IEnumerable<ValidationError>> Validate(Executor executor);
        Task<object> Execute(Executor executor);
    }
}
