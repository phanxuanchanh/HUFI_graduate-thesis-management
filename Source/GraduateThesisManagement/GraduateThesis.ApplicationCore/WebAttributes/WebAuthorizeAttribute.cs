using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Authorization;

namespace GraduateThesis.ApplicationCore.WebAttributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class WebAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private string _role;

    public WebAuthorizeAttribute(string role) : base()
    {
        _role = role;    
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        AccountSessionManager accountSessionManager = new AccountSessionManager(context.HttpContext);
        AccountSession? accountSession = accountSessionManager.GetAccountSession(); 

        if (accountSession == null)
            context.Result = new RedirectToActionResult("ShowUnauthorize", "Authorization", null);

        if (accountSession != null && _role != accountSession.Role)
            context.Result = new RedirectToActionResult("ShowUnauthorize", "Authorization", null);
    }
}
