using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace GraduateThesis.Common.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class WebAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public WebAuthorizeAttribute(string role) : base()
        {
            
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HttpContext httpContext = context.HttpContext;

            context.Result = new UnauthorizedResult();
        }
    }
}
