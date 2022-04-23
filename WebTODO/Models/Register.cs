using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTODO.Models
{
    public class Register
    {
        public int UserId { get; set; }

        [EmailAddress(ErrorMessage = "Provide valid Email Address")]
        [Required(ErrorMessage = "Provide Email Address")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Provide Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Provide ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
