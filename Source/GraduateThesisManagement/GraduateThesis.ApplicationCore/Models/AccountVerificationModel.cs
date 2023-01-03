using System.ComponentModel.DataAnnotations;
using GraduateThesis.ApplicationCore.Enums;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class AccountVerificationModel
{
    public AccountStatus AccountStatus { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string VerificationCode { get; set; }
}
