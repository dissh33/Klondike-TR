using MediatR;

namespace ItemManagementService.Api.Commands;

public  class DeleteByIdCommand : IRequest<int>
{
    public DeleteByIdCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}