using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.Api.Dtos;
using Offers.Api.Queries.OfferItem;

namespace Offers.Admin.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OfferItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public OfferItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OfferItemDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new OfferItemGetByIdQuery(id), ct);
        return new JsonResult(result);
    }

    [HttpGet("GetByPosition")]
    public async Task<ActionResult<IEnumerable<OfferItemDto>>> GetByPosition([FromQuery]Guid positionId, CancellationToken ct)
    {
        var result = await _mediator.Send(new OfferItemGetByPositionQuery(positionId), ct);
        return new JsonResult(result);
    }
}
