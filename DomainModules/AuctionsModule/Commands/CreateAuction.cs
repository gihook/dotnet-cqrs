using System.Collections.Generic;
using System.Linq;
using Action.Interfaces;
using Action.Models;

namespace AuctionModule.Commands
{
    public class CreateAuction : Command<int>
    {
        public override IEnumerable<ValidationError> Validate(Executor executor)
        {
            return Enumerable.Empty<ValidationError>();
        }

        protected override int ExecuteInternal(Executor executor)
        {
            return 1;
        }
    }
}
