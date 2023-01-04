﻿using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

#nullable disable

namespace GraduateThesis.ApplicationCore.WebAttributes;

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
        Controller controller = (filterContext.Controller as Controller);

        AccountSessionManager accountSessionManager = new AccountSessionManager(filterContext.HttpContext);
        AccountSession accountSession = accountSessionManager.GetAccountSession();
        if (accountSession == null)
            throw new Exception("Session must not be null!");

        controller.ViewData["AccountModel"] = JsonConvert.DeserializeObject(accountSession.AccountModel.ToString(), _type);
    }
}