using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class TellerPostingMap : ClassMap<TellerPosting>
    {
        public TellerPostingMap()
        {
            Id(x => x.Id);
            Map(x => x.PostAmount);
            Map(x => x.Narration);
            Map(x => x.PostType);
            References(x => x.CustomerAccount).Not.LazyLoad();
            References(x => x.CustomerSavingAccount).Not.LazyLoad();
            References(x => x.CustomerCurrentAccount).Not.LazyLoad();
            References(x => x.Till).Not.LazyLoad();
            Map(x => x.TransactionDate);
            Table("TellerPosting");
        }
    }
}