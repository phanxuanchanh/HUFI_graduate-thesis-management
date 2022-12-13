using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Common.WebAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class PageNameAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Type controllerType = filterContext.Controller.GetType();
            PropertyInfo propertyInfo = controllerType.GetProperty("ViewData");
            ViewDataDictionary viewData = (ViewDataDictionary)propertyInfo.GetValue(filterContext.Controller);
            viewData["PageName"] = Name;
        }
    }
}
