using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCTut.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }

    }
}