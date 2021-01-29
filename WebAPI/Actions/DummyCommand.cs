using System.Collections.Generic;
using System.Linq;
using WebAPI.ActionInterfaces;
using WebAPI.ActionModels;

namespace WebAPI.Actions
{
    public class DummyCommand : Command
    {
        public int Count { get; set; }
        public string Title { get; set; }

        public override object Execute(Executor executor)
        {
            return $"Is it working? with count: {Count} and title {Title}";
        }

        public override IEnumerable<ValidationError> Validate(Executor executor)
        {
            return Enumerable.Empty<ValidationError>();
        }
    }
}
