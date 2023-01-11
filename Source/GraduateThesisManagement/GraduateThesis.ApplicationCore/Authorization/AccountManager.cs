using GraduateThesis.ApplicationCore.Context;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Session;
using Microsoft.AspNetCore.Http;

#nullable disable

namespace GraduateThesis.ApplicationCore.Authorization;

public class AccountManager: IAccountManager
{
    private readonly AppDbContext _context;
    private HttpContext _httpContext;

    public AccountManager(AppDbContext context) 
    {
        _context = context;
    }


    public AccountSession GetSession()
    {
        SessionManager sessionManager = new SessionManager(_httpContext);
        return sessionManager.GetSession<AccountSession>("account-session");
    }

    public void SetHttpContext(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    public void SetSession(AccountSession accountSession)
    {
        if (accountSession == null)
            throw new Exception("'AccountSession' must not be null!");

        SessionManager sessionManager = new SessionManager(_httpContext);
        sessionManager.SetSession<AccountSession>("account-session", accountSession);
    }
}
