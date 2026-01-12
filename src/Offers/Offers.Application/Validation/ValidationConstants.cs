using Offers.Domain.Enums;

namespace Offers.Application.Validation;

public static class ValidationConstants
{
    internal static readonly int OFFER_STATUS_MAX_RANGE = Enum.GetValues<OfferStatus>().Length - 1;

    internal static readonly int OFFER_TYPE_MAX_RANGE = Enum.GetValues<OfferType>().Length - 1;
    internal static readonly int OFFER_POSITION_TYPE_MAX_RANGE = Enum.GetValues<OfferPositionType>().Length - 1;
    internal static readonly int OFFER_ITEM_TYPE_MAX_RANGE = Enum.GetValues<OfferItemType>().Length - 1;
}
