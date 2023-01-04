using GraduateThesis.ApplicationCore.Enums;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class SignInResultModel
{
    public SignInStatus Status { get; set; }
    public string Message { get; set; }
}
