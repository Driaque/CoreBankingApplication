namespace Core
{
    public class LoanAccountConfig : Entity
    {
        public virtual double DebitInterestRate { get; set; }
        public virtual GLAccount InterestIncomeGL { get; set; }
        public virtual GLAccount LoanGL { get; set; }
    }
}
