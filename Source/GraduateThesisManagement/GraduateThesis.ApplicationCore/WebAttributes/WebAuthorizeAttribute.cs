using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using GraduateThesis.ApplicationCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using GraduateThesis.ApplicationCore.Context;
using GraduateThesis.ApplicationCore.Models;

#nullable disable

namespace GraduateThesis.ApplicationCore.WebAttributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class WebAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private string _role;

    public WebAuthorizeAttribute(string role) : base()
    {
        _role = role;    
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        ControllerActionDescriptor actionDescriptor = (context.ActionDescriptor as ControllerActionDescriptor);

        AppDbContext dbContext = AppDbContext.CreateContext();
        IRoleManager roleManager = new RoleManager(dbContext);
        IAccountManager accountManager = new AccountManager(dbContext);

        accountManager.SetHttpContext(context.HttpContext);
        AccountSession accountSession = accountManager.GetSession();

        bool isValid = await roleManager.IsValidAsync(accountSession.UserId, actionDescriptor.ActionName, actionDescriptor.ControllerName);
        if (!isValid)
        {
            context.Result = new RedirectToActionResult("ShowUnauthorize", "Authorization", null);
        }
    }

}
