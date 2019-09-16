using Core;

namespace Map
{
    public class LoanAccountMap : EntityMap<LoanAccount>
    {
        public LoanAccountMap()
        {

            References(x => x.CustomerAccount).Not.LazyLoad();
            Map(x => x.Duration);
            Map(x => x.InterestAmount);
            Map(x => x.PrincipalAmount);
            Map(x => x.LoanAccountNumber);

        }
    }
}
