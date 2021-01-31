using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action.Interfaces;
using Action.Models;
using Models.AuctionsModule;
using Services.Interfaces;

namespace AuctionModule.Commands
{
    public class CreateAuction : Command<Auction>
    {
        private readonly IGenericService<int, Auction> _auctionService;

        public string Name { get; set; }
        public string Description { get; set; }
        public int InitialPrice { get; set; }

        public CreateAuction(IGenericService<int, Auction> auctionService)
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
            System.Console.Write("Executing action CreateAuction");

            var auction = new Auction
            {
                Name = Name,
                Description = Description,
                CurrentPrice = InitialPrice
            };

            var result = await _auctionService.SaveAsync(auction);

            return result;
        }
    }
}
