using System.Collections.Generic;
using WebAPI.ActionModels;

namespace WebAPI.ActionInterfaces
{
    public interface IAction
    {
        IEnumerable<ValidationError> Validate(Executor executor);
        object Execute(Executor executor);
    }
}
