using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class LoanAccountMap : ClassMap<LoanAccount>
    {
        public LoanAccountMap()
        {
            Id(x => x.Id);
            References(x => x.CustomerAccount).Not.LazyLoad();
            Map(x => x.Duration);
            Map(x => x.InterestAmount);
            Map(x => x.PrincipalAmount);
            Map(x => x.LoanAccountNumber);
            //Map(x => x.FinancialPostingDate);
            //Map(x => x.PaymentType);
            //Map(x => x.FinancialPostingDateUpdated);
        }
    }
}