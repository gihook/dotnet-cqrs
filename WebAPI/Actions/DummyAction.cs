using System.Collections.Generic;
using System.Linq;
using WebAPI.ActionInterfaces;
using WebAPI.ActionModels;

namespace WebAPI.Actions
{
    public class DummyAction : Command
    {
        public override object Execute(Executor executor)
        {
            return "Is it working?";
        }

        public override IEnumerable<ValidationError> Validate(Executor executor)
        {
            return Enumerable.Empty<ValidationError>();
        }
    }
}
