using System;
using System.ComponentModel.DataAnnotations;

namespace MVCTut.Models
{
    public class TellerPosting
    {
        public virtual int Id { get; set; }
        [Range(1, 200000, ErrorMessage = "post amount must be greater than zero"), RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter a Valid number")]
        public virtual double PostAmount { get; set; }
        [Required, StringLength(60)]
        public virtual string Narration { get; set; }
        public virtual CustomerAccount CustomerAccount { get; set; }
        public virtual GLAccount CustomerSavingAccount { get; set; }
        public virtual GLAccount CustomerCurrentAccount { get; set; }

        public virtual PostType PostType { get; set; }
        public virtual TellerManagement Till { get; set; }
        public virtual DateTime TransactionDate { get; set; }
    }
}