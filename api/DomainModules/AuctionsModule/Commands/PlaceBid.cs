using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action.Interfaces;
using Action.Models;
using Models.AuctionsModule;
using Services.Interfaces;

namespace AuctionModule.Commands
{
    public class PlaceBid : Command<Auction>
    {
        public int Id { get; set; }
        public int PriceValue { get; set; }

        private readonly IGenericService<int, Auction> _auctionService;

        public PlaceBid(IGenericService<int, Auction> auctionService)
        {
            _auctionService = auctionService;
        }

        public override async Task<IEnumerable<ValidationError>> Validate(Executor executor)
        {
            await Task.CompletedTask;
            return Enumerable.Empty<ValidationError>();
        }

        protected override async Task<Auction> ExecuteInternal(Executor executor)
        {
            var auction = await _auctionService.GetById(Id);
            auction.CurrentPrice = PriceValue;
            var result = await _auctionService.UpdateAsync(auction);

            return result;
        }
    }
}
