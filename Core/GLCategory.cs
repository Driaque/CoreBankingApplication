using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class GLCategory : Entity
    {
        [Required]
        public virtual GLCategoryType CategoryType { get; set; }
        [Required, StringLength(25)]
        public virtual string Description { get; set; }
        [Required, StringLength(25), RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        public virtual string GLCategoryName { get; set; }
    }
}
