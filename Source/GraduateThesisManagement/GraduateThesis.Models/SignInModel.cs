using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Models
{
    public class SignInModel
    {
        [Required]
        [RegularExpression("^[0-9]{10}$")]
        public string Code { get; set; }

        [Required]
        [DataType(DataType.Password)]                
        public string Password { get; set; }
    }
}
