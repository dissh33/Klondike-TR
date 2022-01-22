using MediatR;

namespace ItemManagementService.Api.Commands;

public  class IconDeleteCommand : IRequest<int>
{
    public IconDeleteCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}