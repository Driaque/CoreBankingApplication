using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class CurrentAccountConfig : Entity
    {
        [Required]
        public virtual double CreditInterestRate { get; set; }
        [Required]
        public virtual double MinimumBalance { get; set; }
        public virtual GLAccount InterestExpenseGL { get; set; }
        public virtual double COT { get; set; }
        public virtual GLAccount COTIncomeGL { get; set; }
        public virtual GLAccount CustomerCurrentAccount { get; set; }

        public CurrentAccountConfig()
        {
            CustomerCurrentAccount
                = new GLAccount();
        }
    }
}
