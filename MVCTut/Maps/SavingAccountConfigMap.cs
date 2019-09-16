using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class SavingAccountConfigMap : ClassMap<SavingAccountConfig>
    {
        public SavingAccountConfigMap()
        {
            Id(x => x.Id);
            Map(x => x.MinimumBalance);
            Map(x => x.CreditInterestRate);
            References(x => x.InterestExpenseGL).Not.LazyLoad();
            References(x => x.CustomerSavingAccount).Not.LazyLoad();

            Table("SavingAccConfig");
        }
    }
}