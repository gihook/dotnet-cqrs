using System.Collections.Generic;
using WebAPI.ActionModels;

namespace WebAPI.ActionInterfaces
{
    public interface IAction<T>
    {
        IEnumerable<ValidationError> Validate(Executor executor);
        T Execute(Executor executor);
    }
}
