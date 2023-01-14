using System.Net;
using System.Net.Mail;

namespace GraduateThesis.ApplicationCore.Email;

public class SmtpService : IEmailService
{
    private SmtpClient _smtp;
    private string _user;
    private bool disposedValue;

    public SmtpService(string host, int port, string user, string password, bool enableSsl)
    {
        _smtp = new SmtpClient();
        _smtp.UseDefaultCredentials = false;

        _smtp.Host = host;
        _smtp.Port = port;
        _smtp.EnableSsl = enableSsl;
        _smtp.Credentials = new NetworkCredential(user, password);

        _user = user;
    }

    public void Send(string recipients, string subject, string content)
    {
        _smtp.Send(_user, recipients, subject, content);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {

            }

            _smtp.Dispose();
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
