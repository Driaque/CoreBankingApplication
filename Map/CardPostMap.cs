using Core;

namespace Map
{
    public class CardPostMap : EntityMap<CardPost>
    {
        public CardPostMap()
        {
            Map(x => x.Amount);
            Map(x => x.CardPan);
            Map(x => x.TransactionType);//.CustomType<TransactionType>();
            References(x => x.CustomerAccount).Not.LazyLoad();
            Map(x => x.FinancialDate);
            References(x => x.TillAccount).Not.LazyLoad();
            References(x => x.Account2).Not.LazyLoad();
            Map(x => x.OriginalDataElement);
            Map(x => x.IsReversed);
        }
    }
}
