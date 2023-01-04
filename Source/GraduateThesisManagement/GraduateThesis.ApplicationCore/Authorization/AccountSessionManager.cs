using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Session;
using Microsoft.AspNetCore.Http;

namespace GraduateThesis.ApplicationCore.Authorization;

public class AccountSessionManager : SessionManager
{
    public AccountSessionManager(HttpContext httpContext) : base(httpContext)
    {
    }

    public AccountSession? GetAccountSession()
    {
        return GetSession<AccountSession>("account-session");
    }

    public void SetAccountSession(AccountSession? accountSession)
    {
        if (accountSession == null)
            throw new Exception("'AccountSession' must not be null!");

        accountSession.LastSignInTime = DateTime.Now;
        SetSession<AccountSession>("account-session", accountSession);
    }
}
