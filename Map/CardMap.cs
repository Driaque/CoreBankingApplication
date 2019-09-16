using Core;

namespace Map
{
    public class CardMap : EntityMap<Card>
    {
        public CardMap()
        {
            Map(x => x.CVV2);
            References(x => x.Customeraccount).Not.LazyLoad();
            Map(x => x.ExpiryDate);
            Map(x => x.PAN);
        }
    }
}
