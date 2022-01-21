using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Responses;
using MediatR;

namespace ItemManagementService.Application.ApplicationLogic.Commands;

public class IconAddHandler : IRequestHandler<IconAddCommand, IconAddResponse>
{
    public async Task<IconAddResponse> Handle(IconAddCommand request, CancellationToken cancellationToken)
    {
        return new IconAddResponse();
    }
}
