using Fintacharts.Application.Common.Interfaces;
using MediatR;

namespace Fintacharts.Application.Features.Forex
{
    public class DeleteQuery : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteQueryHandler(IForex forex) : IRequestHandler<DeleteQuery, bool>
    {
        private readonly IForex _forex = forex;
        public async Task<bool> Handle(DeleteQuery request, CancellationToken cancellationToken)
        {
            return await _forex.DeleteAsset(request.Id);
        }
    }
}
