namespace KlondikeTR.ViewModels
{
    public class OfferItemModel
    {
        public Guid? OfferPositionId { get; set; }
        public Guid TradableItemId { get; set; }

        public string? Title { get; set; }
        public int Amount { get; set; }
        public int Type { get; set; }
    }
}
