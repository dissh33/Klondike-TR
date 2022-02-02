using ItemManagementService.Api.Commands;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.RequestsLogic.Commands;

public class IconDeleteHandler : IRequestHandler<IconDeleteCommand, int>
{
    private readonly IUnitOfWork _uow;

    public IconDeleteHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(IconDeleteCommand request, CancellationToken ct)
    {
        var result = await _uow.IconRepository!.Delete(request.Id, ct);

        _uow.Commit();

        return result;
    }
}
