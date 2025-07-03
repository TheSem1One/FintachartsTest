using Fintacharts.Application.Common.Interfaces;
using Fintacharts.Domain.Entity;
using MediatR;

namespace Fintacharts.Application.Features.Forex
{
    public class GetAllQuery : IRequest<IList<Asset>> {}

    public class GetAllQueryHandler(IForex forex) : IRequestHandler<GetAllQuery, IList<Asset>>
    {
        private readonly IForex _forex=forex;
        public async Task<IList<Asset>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
           var assets = await _forex.GetAll();
           return assets.ToList();
        }
    }
}
