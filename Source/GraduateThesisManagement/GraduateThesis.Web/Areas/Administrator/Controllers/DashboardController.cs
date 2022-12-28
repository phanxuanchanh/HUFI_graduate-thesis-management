using GraduateThesis.Common.WebAttributes;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Route("dashboard")]
    [HandleException]
    public class DashboardController : Controller
    {
        [Route("Index")]
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
