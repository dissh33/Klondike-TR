using FluentValidation;
using ItemManagementService.Api.Commands.Material;

namespace ItemManagementService.Api.Validators;

public class MaterialAddValidator : AbstractValidator<MaterialAddCommand>
{
    public MaterialAddValidator()
    {
        
    }
}

public class MaterialUpdateValidator : AbstractValidator<MaterialUpdateCommand>
{
    public MaterialUpdateValidator()
    {
        
    }
}