using ItemManagementService.Api.Commands;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Commands.Icon;

public class IconDeleteHandler : IRequestHandler<DeleteByIdCommand, int>
{
    private readonly IUnitOfWork _uow;

    public IconDeleteHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(DeleteByIdCommand request, CancellationToken ct)
    {
        var result = await _uow.IconRepository!.Delete(request.Id, ct);

        _uow.Commit();

        return result;
    }
}
