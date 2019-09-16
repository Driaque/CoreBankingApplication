using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class GLPostingMap : ClassMap<GLPosting>
    {
        public GLPostingMap()
        {
            Id(x => x.Id);
            Map(x => x.PostAmount);
            Map(x => x.Narration);
            References(x => x.GLAccountToDebit).Not.LazyLoad();
            References(x => x.GLAccountToCredit).Not.LazyLoad();
            References(x => x.User);
            Map(x => x.TransactionDate);
            Table("LedgerPosting");
        }
    }
}