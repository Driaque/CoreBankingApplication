using System;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class GLPosting : Entity
    {
        [Range(1, 200000000000, ErrorMessage = "Post amount must be greater than zero", ErrorMessageResourceName = "Cannot be More than xxxxx ammount"), RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter a Valid  number")]
        public virtual double PostAmount { get; set; }
        public virtual string Narration { get; set; }
        [Display(Name = "Account Debited")]
        public virtual GLAccount GLAccountToDebit { get; set; }
        [Display(Name = "Account Credited")]
        public virtual GLAccount GLAccountToCredit { get; set; }
        public virtual User User { get; set; }
        public virtual DateTime TransactionDate { get; set; }
    }
}
