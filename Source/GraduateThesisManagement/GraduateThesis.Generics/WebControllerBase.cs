﻿using GraduateThesis.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Generics
{
    public class WebControllerBase : Controller
    {
        [NonAction]
        protected void AddViewData(string pageName)
        {
            ViewData["PageName"] = pageName;
        }

        [NonAction]
        protected void AddViewData(string statusCode, string message)
        {
            ViewData["StatusCode"] = statusCode;
            ViewData["Message"] = message;
        }

        [NonAction]
        protected void AddViewData(string pageName, string statusCode, string message)
        {
            ViewData["PageName"] = pageName;
            AddViewData(statusCode, message);
        }

        [NonAction]
        protected void AddViewData(string pageName, DataResponse dataResponse)
        {
            ViewData["PageName"] = pageName;
            ViewData["Status"] = dataResponse.Status.ToString();

            if (string.IsNullOrEmpty(dataResponse.Message))
            {
                if (dataResponse.Status == DataResponseStatus.Success)
                    ViewData["Message"] = "";
                else if (dataResponse.Status == DataResponseStatus.NotFound)
                    ViewData["Message"] = "";
                else if (dataResponse.Status == DataResponseStatus.AlreadyExists)
                    ViewData["Message"] = "";
                else
                    ViewData["Message"] = "";
            }
            else
            {
                ViewData["Message"] = dataResponse.Message;
            }
        }

        [NonAction]
        protected void AddViewData(string pageName, DataResponseStatus dataResponseStatus)
        {
            ViewData["PageName"] = pageName;
            ViewData["Status"] = dataResponseStatus.ToString();

            if (dataResponseStatus == DataResponseStatus.Success)
                ViewData["Message"] = "";
            else if (dataResponseStatus == DataResponseStatus.NotFound)
                ViewData["Message"] = "";
            else if (dataResponseStatus == DataResponseStatus.AlreadyExists)
                ViewData["Message"] = "";
            else
                ViewData["Message"] = "";
        }

        [NonAction]
        protected void AddViewData(string pageName, SignInResultModel signInResultModel)
        {
            ViewData["PageName"] = pageName;
            ViewData["Status"] = signInResultModel.Status.ToString();

            if (string.IsNullOrEmpty(signInResultModel.Message))
            {
                if (signInResultModel.Status == SignInStatus.Success)
                    ViewData["Message"] = "";
                else if (signInResultModel.Status == SignInStatus.WrongPassword)
                    ViewData["Message"] = "";
                else if (signInResultModel.Status == SignInStatus.NotFound)
                    ViewData["Message"] = "";
                else
                    ViewData["Message"] = "";
            }
            else
            {
                ViewData["Message"] = signInResultModel.Message;
            }
        }

        [NonAction]
        protected void AddTempData(string pageName, SignInStatus signInStatus)
        {
            TempData["PageName"] = pageName;
            TempData["Status"] = signInStatus.ToString();

            if (signInStatus == SignInStatus.Success)
                TempData["Message"] = "";
            else if (signInStatus == SignInStatus.WrongPassword)
                TempData["Message"] = "";
            else if (signInStatus == SignInStatus.NotFound)
                TempData["Message"] = "";
            else
                TempData["Message"] = "";
        }

        [NonAction]
        protected void AddTempData(string pageName, SignInResultModel signInResultModel)
        {
            TempData["PageName"] = pageName;
            TempData["Status"] = signInResultModel.Status.ToString();

            if (string.IsNullOrEmpty(signInResultModel.Message))
            {
                if (signInResultModel.Status == SignInStatus.Success)
                    TempData["Message"] = "";
                else if (signInResultModel.Status == SignInStatus.WrongPassword)
                    TempData["Message"] = "";
                else if (signInResultModel.Status == SignInStatus.NotFound)
                    TempData["Message"] = "";
                else
                    TempData["Message"] = "";
            }
            else
            {
                TempData["Message"] = signInResultModel.Message;
            }
        }

        [NonAction]
        protected void AddTempData(string pageName, DataResponse dataResponse)
        {
            TempData["PageName"] = pageName;
            TempData["Status"] = dataResponse.Status.ToString();

            if (string.IsNullOrEmpty(dataResponse.Message))
            {
                if (dataResponse.Status == DataResponseStatus.Success)
                    TempData["Message"] = "";
                else if (dataResponse.Status == DataResponseStatus.NotFound)
                    TempData["Message"] = "";
                else if (dataResponse.Status == DataResponseStatus.AlreadyExists)
                    TempData["Message"] = "";
                else
                    TempData["Message"] = "";
            }
            else
            {
                TempData["Message"] = dataResponse.Message;
            }
        }
    }
}
