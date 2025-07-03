using Fintacharts.Application.DTO.Assets;
using Fintacharts.Application.Features.Market;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintacharts.API.Controllers
{
    public class MarketController(IMediator mediator) : ApiController
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<ActionResult<AssetPriceDto>> GetPrice(int id)
        {
            var result = await _mediator.Send(new GetPriceByIdQuery{Id = id});
            return Ok(result);
        }
    }
}
