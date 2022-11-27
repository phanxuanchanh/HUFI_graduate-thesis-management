using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Controllers
{
    [Route("dashboard")]
    public class DashboardController : Controller
    {
        [Route("simple")]
        public IActionResult GetSimple()
        {
            return View();
        }
    }
}
