using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Models
{
    public class AccountVerificationModel
    {
        public AccountStatus AccountStatus { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string VerificationCode { get; set; }
    }
}
