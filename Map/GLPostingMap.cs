using Core;

namespace Map
{
    public class GLPostingMap : EntityMap<GLPosting>
    {
        public GLPostingMap()
        {
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
