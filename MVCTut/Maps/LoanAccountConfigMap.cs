using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class LoanAccountConfigMap : ClassMap<LoanAccountConfig>
    {
        public LoanAccountConfigMap()
        {
            Id(x => x.Id);
            Map(x => x.DebitInterestRate);
            References(x => x.InterestIncomeGL).Not.LazyLoad();
            References(x => x.LoanGL).Not.LazyLoad();
            Table("LoanAccConfig");
        }
    }
}