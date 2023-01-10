
namespace GraduateThesis.ApplicationCore.Email;

public interface IEmailService : IDisposable
{
    void Send(string email, string subject, string content);
}
