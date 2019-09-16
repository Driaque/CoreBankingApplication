using System.ComponentModel.DataAnnotations;

namespace MVCTut.Core
{
    public class SavingAccountConfig : Entity
    {
        [Required]
        public virtual double CreditInterestRate { get; set; }
        [Required]
        public virtual double MinimumBalance { get; set; }
        public virtual GLAccount InterestExpenseGL { get; set; }
        public virtual GLAccount CustomerSavingAccount { get; set; }

        public SavingAccountConfig()
        {
            CustomerSavingAccount = new GLAccount();
        }
    }
}