using System;
using System.ComponentModel.DataAnnotations;

namespace MVCTut.Models
{
    public class GLPosting
    {
        public virtual int Id { get; set; }
        [Range(1, 20000000000000, ErrorMessage = "post amount must be greater than zero"), RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter a Valid  number")]
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