using Google.Apis.Gmail.v1;

namespace GraduateThesis.ApplicationCore.Email;

public class GoogleMailService : EmailService
{
    private GmailService _gmailService;

    public GoogleMailService() : base()
    {
        _gmailService = new GmailService();
    }

    public void Send()
    {

    }
}
