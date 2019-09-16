using Core;

namespace Map
{
    public class LoanAccountConfigMap : EntityMap<LoanAccountConfig>
    {
        public LoanAccountConfigMap()
        {

            Map(x => x.DebitInterestRate);
            References(x => x.InterestIncomeGL).Not.LazyLoad();
            References(x => x.LoanGL).Not.LazyLoad();

            Table("LoanAccConfig");
        }
    }
}
