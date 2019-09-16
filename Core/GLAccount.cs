using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class GLAccount : Entity
    {
        [Required, Display(Name = "Account"), RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        public virtual string GLAccountName { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual GLCategory GLCategory { get; set; }
        [Display(Name = "GL Account Code")]
        public virtual string GLCode { get; set; }
        public virtual double AccountBalance { get; set; }
        public virtual bool IsAssigned { get; set; }
    }
}
