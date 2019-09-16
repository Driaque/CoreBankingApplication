using Core;

namespace Map
{
    public class SavingAccountConfigMap : EntityMap<SavingAccountConfig>
    {
        public SavingAccountConfigMap()
        {

            Map(x => x.MinimumBalance);
            Map(x => x.CreditInterestRate);
            References(x => x.InterestExpenseGL).Not.LazyLoad();
            References(x => x.CustomerSavingAccount).Not.LazyLoad();
            Table("SavingAccConfig");
        }
    }
}
