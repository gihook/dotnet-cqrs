using System.Collections.Generic;
using WebAPI.ActionModels;

namespace WebAPI.ActionInterfaces
{
    public abstract class Command : IAction
    {
        public abstract object Execute(Executor executor);
        public abstract IEnumerable<ValidationError> Validate(Executor executor);
    }
}
