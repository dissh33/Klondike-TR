using Items.Api.Dtos;
using Items.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Items.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradableItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TradableItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradableItemDto>>> GetAllActive(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAllAvailableItemsQuery(), ct);
            return new JsonResult(result);
        }
    }
}
