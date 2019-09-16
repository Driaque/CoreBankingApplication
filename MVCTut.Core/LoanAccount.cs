using System.ComponentModel.DataAnnotations;

namespace MVCTut.Core
{
    public class LoanAccount : Entity
    {
        public virtual CustomerAccount CustomerAccount { get; set; }
        [Range(1, 200000000000, ErrorMessage = "Principal Amount must be more than 0"), RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter a Valid number")]
        public virtual double PrincipalAmount { get; set; }
        [DataType(DataType.Duration), Display(Name = "Duration (days)"), Range(1, 1000, ErrorMessage = "Duration must me more than a day!")]
        public virtual int Duration { get; set; }

        public virtual double InterestAmount { get; set; }
        public virtual string LoanAccountNumber { get; set; }
        //public virtual string PaymentType { get; set; }
        //public virtual DateTime FinancialPostingDate { get; set; }
        //public virtual DateTime FinancialPostingDateUpdated { get; set; }
    }
}