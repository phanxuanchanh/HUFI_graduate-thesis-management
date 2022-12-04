using GraduateThesis.Models;
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
            ViewData["Status"] = DataResponseStatus.NotFound.ToString();

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
    }
}
