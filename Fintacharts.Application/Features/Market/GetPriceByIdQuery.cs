using Fintacharts.Application.Common.Interfaces;
using Fintacharts.Application.DTO.Assets;
using MediatR;

namespace Fintacharts.Application.Features.Market
{
    public class GetPriceByIdQuery : IRequest<AssetPriceDto>
    {
        public int Id { get; set; }
    }

    public class GetPriceByIdQueryHandler(IMarket market) : IRequestHandler<GetPriceByIdQuery, AssetPriceDto>
    {
        private readonly IMarket _market = market;
        public async Task<AssetPriceDto> Handle(GetPriceByIdQuery request, CancellationToken cancellationToken)
        {
           var result= await _market.GetPrice(request.Id);
           return result;
        }
    }
}
