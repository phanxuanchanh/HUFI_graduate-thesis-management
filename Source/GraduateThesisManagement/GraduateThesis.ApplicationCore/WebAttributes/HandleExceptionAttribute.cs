using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GraduateThesis.ApplicationCore.WebAttributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class HandleExceptionAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.Result = new RedirectToActionResult("ShowError", "Error", null);
    }
}
