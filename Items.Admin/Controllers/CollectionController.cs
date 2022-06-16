using Items.Api.Commands;
using Items.Api.Commands.Collection;
using Items.Api.Dtos;
using Items.Api.Queries.Collection;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Items.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CollectionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionDto>>> Get(CancellationToken ct)
        {
            var result = await _mediator.Send(new CollectionGetAllQuery(), ct);
            return new JsonResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionDto>> Get(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new CollectionGetByIdQuery(id), ct);
            return new JsonResult(result);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CollectionDto>>> GetByFilter([FromQuery] CollectionGetByFilterQuery request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<CollectionDto>> Post([FromForm] CollectionAddCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPut("update")]
        public async Task<ActionResult<CollectionDto>> Put([FromForm] CollectionUpdateCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPut("update/name")]
        public async Task<ActionResult<CollectionDto>> UpdateName([FromForm] CollectionUpdateNameCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPut("update/icon")]
        public async Task<ActionResult<CollectionDto>> UpdateIcon([FromForm] CollectionUpdateIconCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPut("update/status")]
        public async Task<ActionResult<CollectionDto>> UpdateStatus([FromForm] CollectionUpdateStatusCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new DeleteByIdCommand(id), ct);
            return new JsonResult(result);
        }
    }
}
