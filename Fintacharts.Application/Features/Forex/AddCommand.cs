using Fintacharts.Application.Common.Interfaces;
using Fintacharts.Application.DTO.Assets;
using Fintacharts.Domain.DTO;
using MediatR;

namespace Fintacharts.Application.Features.Forex
{
    public class AddCommand : IRequest<bool>
    {
        public string Symbol { get; set; }
    }

    public class AddCommandHandler(IForex forex) : IRequestHandler<AddCommand, bool>
    {
        private readonly IForex _forex = forex;
        public async Task<bool> Handle(AddCommand request, CancellationToken cancellationToken)
        {
            return await _forex.AddAsset(new AssetDto{Symbol = request.Symbol});
        }
    }
}
