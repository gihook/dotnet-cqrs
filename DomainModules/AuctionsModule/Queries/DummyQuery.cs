using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action.Interfaces;
using Action.Models;

namespace AuctionModule.Queries
{
    public class DummyQuery : Query<string>
    {
        public int Count { get; set; }
        public string Title { get; set; }

        protected override Task<string> ExecuteInternal(Executor executor)
        {
            var result = $"Is it working? with count: {Count} and title {Title}";
            return Task.FromResult(result);
        }

        public override async Task<IEnumerable<ValidationError>> Validate(Executor executor)
        {
            await Task.CompletedTask;
            return Enumerable.Empty<ValidationError>();
        }
    }
}

