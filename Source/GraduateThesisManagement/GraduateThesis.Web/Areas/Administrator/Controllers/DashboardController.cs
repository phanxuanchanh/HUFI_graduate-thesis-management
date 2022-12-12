using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Route("dashboard")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ExecQuery()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExecQuery(string query)
        {
            return View();
        }
    }
}
