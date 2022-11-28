using GraduateThesis.Common.Authorization;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Controllers
{
    [Route("dashboard")]
    public class DashboardController : Controller
    {
        [Route("simple")]
        [WebAuthorize(AccountRole.Administrator)]
        public IActionResult GetSimple()
        {
            return View();
        }
    }
}
