using System;
using System.ComponentModel.DataAnnotations;


namespace MVCTut.Models
{
    [Serializable]
    public class User
    {
        public virtual int Id { get; set; }

        [Required, StringLength(25), Display(Name = "Firstname"), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public virtual string FirstName { get; set; }

        [Required, StringLength(25), Display(Name = "Lastname"), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public virtual string LastName { get; set; }

        public virtual string FullName
        {
            get { return FirstName + " " + LastName; }
            set { }
        }

        [Required, StringLength(25), Display(Name = "Username"), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public virtual string UserName { get; set; }
        [StringLength(11), MaxLength(18), MinLength(5), DataType(DataType.Password)]
        public virtual string Password { get; set; }

        [DataType(DataType.EmailAddress), Required, StringLength(60)]
        public virtual string Email { get; set; }
        [Required]
        public virtual Branch Branch { get; set; }

        [DataType(DataType.PhoneNumber), RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter a Valid phone number"), StringLength(11)]
        public virtual string PhoneNumber { get; set; }
        public virtual bool IsAssigned { get; set; }
        [Display(Name = "User Role")]
        public virtual bool IsAdmin { get; set; }

    }
}