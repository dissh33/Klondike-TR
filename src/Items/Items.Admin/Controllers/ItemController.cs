using Items.Api.Dtos;
using Items.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Items.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradableItemDto>>> Get(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAllAvailableItemsQuery(), ct);
            return new JsonResult(result);
        }
    }
}
