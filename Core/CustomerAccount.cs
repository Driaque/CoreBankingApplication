using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class CustomerAccount : Entity
    {

        public virtual Customer Customer { get; set; }
        public virtual Branch Branch { get; set; }
        [StringLength(25), Display(Name = "Customer Account Name"), RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        public virtual string AccountName { get; set; }
        public virtual double AccountBalance { get; set; }
        public virtual AccountType AccountType { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string AccountStatus { get; set; }
        public virtual bool IsClosed { get; set; }
    }
}
