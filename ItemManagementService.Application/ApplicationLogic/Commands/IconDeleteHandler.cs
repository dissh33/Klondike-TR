using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Responses;
using MediatR;

namespace ItemManagementService.Application.ApplicationLogic.Commands;

public class IconDeleteHandler : IRequestHandler<IconDeleteCommand, IconDeleteResponse>
{
    public async Task<IconDeleteResponse> Handle(IconDeleteCommand request, CancellationToken cancellationToken)
    {
        return new IconDeleteResponse();
    }
}
