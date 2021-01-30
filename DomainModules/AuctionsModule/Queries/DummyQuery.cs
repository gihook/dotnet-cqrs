using System.Collections.Generic;
using System.Linq;
using Action.Interfaces;
using Action.Models;

namespace AuctionModule.Queries
{
    public class DummyQuery : Query<string>
    {
        public int Count { get; set; }
        public string Title { get; set; }

        protected override string ExecuteInternal(Executor executor)
        {
            return $"Is it working? with count: {Count} and title {Title}";
        }

        public override IEnumerable<ValidationError> Validate(Executor executor)
        {
            return Enumerable.Empty<ValidationError>();
        }
    }
}

