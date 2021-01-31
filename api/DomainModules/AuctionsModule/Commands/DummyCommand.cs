using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action.Interfaces;
using Action.Models;

namespace AuctionModule.Commands
{
    public class Result
    {
        public string Message { get; set; }
        public IEnumerable<string> Scopes { get; set; }
    }

    public class DummyCommand : Command<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Scopes { get; set; }

        protected override Task<Result> ExecuteInternal(Executor executor)
        {
            var message = $"Is it working? with name: {Name} and id: {Id}";
            var result = new Result
            {
                Message = message,
                Scopes = Scopes
            };

            return Task.FromResult(result);
        }

        public override async Task<IEnumerable<ValidationError>> Validate(Executor executor)
        {
            await Task.CompletedTask;
            return Enumerable.Empty<ValidationError>();
        }
    }
}
