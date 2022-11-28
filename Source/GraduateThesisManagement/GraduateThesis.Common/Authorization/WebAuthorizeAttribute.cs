using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;

namespace GraduateThesis.Common.Authorization
{
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
            AccountSession accountSession = GetSession(context.HttpContext, "account-session");
            if (accountSession == null)
                context.Result = new RedirectToActionResult("ShowUnauthorize", "Authorization", null);

            if (accountSession != null && _role != accountSession.Role)
                context.Result = new RedirectToActionResult("ShowUnauthorize", "Authorization", null);
        }

        private AccountSession GetSession(HttpContext httpContext, string sessionKey)
        {
            string json = httpContext.Session.GetString(sessionKey);
            return string.IsNullOrEmpty(json) 
                ? default(AccountSession) 
                : JsonConvert.DeserializeObject<AccountSession>(json);
        }
    }
}
