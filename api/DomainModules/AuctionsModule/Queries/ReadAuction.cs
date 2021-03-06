using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action.Interfaces;
using Action.Models;
using Models.AuctionsModule;
using Services.Interfaces;

namespace AuctionModule.Queries
{
    public class ReadAuction : Query<Auction>
    {

        private readonly IGenericService<int, Auction> _auctionService;

        public int? Id { get; set; }

        public ReadAuction(IGenericService<int, Auction> auctionService)
        {
            _auctionService = auctionService;
        }

        public override Task<IEnumerable<ValidationError>> Validate(Executor executor)
        {
            // NOTE: dummy implementation
            var errors = new List<ValidationError>();
            if (Id == null)
            {
                errors.Add(
                    new ValidationError()
                    {
                        Value = Id,
                        ErrorCode = "MandatoryField",
                        FieldName = "Id"
                    });
            }


            var result = errors.AsEnumerable();

            return Task.FromResult(result);
        }

        protected override async Task<Auction> ExecuteInternal(Executor executor)
        {
            var result = await _auctionService.GetById((int)Id);

            return result;
        }
    }
}
