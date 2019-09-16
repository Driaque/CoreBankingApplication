using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class CurrentAccountConfigMap : ClassMap<CurrentAccountConfig>
    {
        public CurrentAccountConfigMap()
        {
            Id(x => x.Id);
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