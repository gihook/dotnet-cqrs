using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action.Interfaces;
using Action.Models;

namespace AuctionModule.Commands
{
    public class CreateAuction : Command<int>
    {
        public override async Task<IEnumerable<ValidationError>> Validate(Executor executor)
        {
            await Task.CompletedTask;
            return Enumerable.Empty<ValidationError>();
        }

        protected override async Task<int> ExecuteInternal(Executor executor)
        {
            await Task.CompletedTask;
            return 1;
        }
    }
}
