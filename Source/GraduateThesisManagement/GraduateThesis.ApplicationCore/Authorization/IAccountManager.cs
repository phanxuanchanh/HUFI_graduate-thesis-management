using GraduateThesis.ApplicationCore.Models;
using Microsoft.AspNetCore.Http;

namespace GraduateThesis.ApplicationCore.Authorization;

public interface IAccountManager
{
    void SetHttpContext(HttpContext httpContext);
    AccountSession GetSession();
    void SetSession(AccountSession accountSession);
    void RemoveSession();
}
