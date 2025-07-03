using Fintacharts.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Fintacharts.Application.Features.Forex;

namespace Fintacharts.API.Controllers
{
    public class ForexController(IMediator mediator) : ApiController
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IList<Asset>>> GetAll()
        {
            var result = await mediator.Send(new GetAllQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddAsset([FromBody] AddCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsset(int id)
        {
            var result = await _mediator.Send(new DeleteQuery { Id = id });
            return Ok(result);
        }
    }
}
