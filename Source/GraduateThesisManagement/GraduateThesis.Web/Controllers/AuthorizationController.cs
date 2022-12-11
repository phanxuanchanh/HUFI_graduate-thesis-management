using GraduateThesis.Generics;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats.Dml;

namespace GraduateThesis.Web.Controllers
{
    public class AuthorizationController : WebControllerBase
    {
        public ActionResult ShowUnauthorize()
        {
            return View();
        }
    }
}
