using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action.Interfaces;
using Action.Models;
using Models.AuctionsModule;
using Services.Interfaces;

namespace AuctionModule.Queries
{
    public class AllAuctions : Query<IEnumerable<Auction>>
    {
        private readonly IGenericService<int, Auction> _auctionService;

        public AllAuctions(IGenericService<int, Auction> auctionService)
        {
            _auctionService = auctionService;
        }

        public override async Task<IEnumerable<ValidationError>> Validate(Executor executor)
        {
            await Task.CompletedTask;
            return Enumerable.Empty<ValidationError>();
        }

        protected override Task<IEnumerable<Auction>> ExecuteInternal(Executor executor)
        {
            return _auctionService.GetAll();
        }
    }
}
