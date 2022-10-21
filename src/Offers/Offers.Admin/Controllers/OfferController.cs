using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Offers.Api.Commands;
using Offers.Api.Dtos;

namespace Offers.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OfferController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<OfferDto>> Get([FromForm] OfferConstructCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return new JsonResult(result);
        }
    }
}
