using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTODO.Models
{
    public class Login
    {
        [EmailAddress(ErrorMessage = "Provide valid Email")]
        public string Email { get; set; }
        
        [MinLength(5, ErrorMessage = "Provide valid password")]
        public string Password { get; set; }
    }
}
