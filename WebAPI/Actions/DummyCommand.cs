using System.Collections.Generic;
using System.Linq;
using WebAPI.ActionInterfaces;
using WebAPI.ActionModels;

namespace WebAPI.Actions
{
    public class DummyCommand : Command<string>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        protected override string ExecuteInternal(Executor executor)
        {
            return $"Is it working? with name: {Name} and id: {Id}";
        }

        public override IEnumerable<ValidationError> Validate(Executor executor)
        {
            return Enumerable.Empty<ValidationError>();
        }
    }
}
