using System.Collections.Generic;
using Action.Models;

namespace Action.Interfaces
{
    public interface IAction
    {
        IEnumerable<ValidationError> Validate(Executor executor);
        object Execute(Executor executor);
    }
}
