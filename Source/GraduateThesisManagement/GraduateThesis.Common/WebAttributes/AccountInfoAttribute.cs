using GraduateThesis.Common.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Text;

namespace GraduateThesis.Common.WebAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AccountInfoAttribute : ActionFilterAttribute
    {
        private Type _type;

        public AccountInfoAttribute(Type type)
        {
            _type = type;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Type controllerType = filterContext.Controller.GetType();
            PropertyInfo propertyInfo = controllerType.GetProperty("ViewData");
            ViewDataDictionary viewData = (ViewDataDictionary)propertyInfo.GetValue(filterContext.Controller);

            AccountSession accountSession = GetSession(filterContext.HttpContext, "account-session");
            if (accountSession == null)
                throw new Exception("Session must not be null!");

            viewData["AccountModel"] = accountSession.AccountModel;
        }

        private AccountSession GetSession(HttpContext httpContext, string sessionKey)
        {
            string json = httpContext.Session.GetString(sessionKey);

            if (string.IsNullOrEmpty(json))
                return default(AccountSession);

            AccountSession accountSession = JsonConvert.DeserializeObject<AccountSession>(json);
            accountSession.AccountModel = JsonConvert.DeserializeObject(accountSession.AccountModel.ToString(), _type);

            return accountSession;
        }
    }
}
