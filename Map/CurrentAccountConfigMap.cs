using Core;

namespace Map
{
    public class CurrentAccountConfigMap : EntityMap<CurrentAccountConfig>
    {
        public CurrentAccountConfigMap()
        {
            Map(x => x.MinimumBalance);
            Map(x => x.CreditInterestRate);
            References(x => x.InterestExpenseGL).Not.LazyLoad();
            Map(x => x.COT);
            References(x => x.COTIncomeGL).Not.LazyLoad();
            References(x => x.CustomerCurrentAccount).Not.LazyLoad();

            Table("CurrentAccConfig");
        }
    }
}
