using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Models
{
    public class ForgotPasswordModel
    {
        public AccountStatus AccountStatus { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
