using Microsoft.AspNetCore.Mvc.Filters;

namespace GraduateThesis.ApplicationCore.WebAttributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class DynamicWebAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public DynamicWebAuthorizeAttribute()
    {
        
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        
    }
}
