using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Responses;
using MediatR;

namespace ItemManagementService.Application.ApplicationLogic.Commands;

public class IconUpdateHandler : IRequestHandler<IconUpdateCommand, IconUpdateResponse>
{
    public async Task<IconUpdateResponse> Handle(IconUpdateCommand request, CancellationToken cancellationToken)
    {
        return new IconUpdateResponse();
    }
}
