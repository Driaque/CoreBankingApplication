namespace MVCTut.Models
{
    public class LoanAccountConfig
    {
        public virtual int Id { get; set; }
        public virtual double DebitInterestRate { get; set; }
        public virtual GLAccount InterestIncomeGL { get; set; }
        public virtual GLAccount LoanGL { get; set; }

    }
}