namespace GraduateThesis.ApplicationCore.Models;

#nullable disable

public class AccountSession
{
    public object AccountModel { get; set; }
    public string Role { get; set; }
    public DateTime LastSignInTime { get; set; }
}
