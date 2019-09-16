using System.ComponentModel.DataAnnotations;

namespace MVCTut.Core
{
    public class Customer : Entity
    {
        [Required, StringLength(25), Display(Name = "Firstname"), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public virtual string FirstName { get; set; }
        [Required, StringLength(25), Display(Name = "Lastname"), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public virtual string LastName { get; set; }
        public virtual Gender Gender { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public virtual string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
            set
            {

            }
        }
        [DataType(DataType.EmailAddress), Required, StringLength(60)]
        public virtual string Email { get; set; }
        [Required, StringLength(80)]
        public virtual string Address { get; set; }

        [DataType(DataType.PhoneNumber), StringLength(11), RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter a Valid phone number")]
        public virtual string PhoneNumber { get; set; }
    }
}