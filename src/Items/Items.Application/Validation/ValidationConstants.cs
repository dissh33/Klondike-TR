using Items.Domain.Enums;

namespace Items.Application.Validation;

public static class ValidationConstants
{
    internal static readonly int ITEM_STATUS_MAX_RANGE = Enum.GetValues<ItemStatus>().Length - 1;
    internal static readonly int MATERIAL_TYPE_MAX_RANGE = Enum.GetValues<MaterialType>().Length - 1;
}
