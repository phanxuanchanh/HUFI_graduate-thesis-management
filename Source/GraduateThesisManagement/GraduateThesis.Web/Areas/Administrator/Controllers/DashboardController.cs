using GraduateThesis.ApplicationCore.AppController;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Route("dashboard")]
    public class DashboardController : WebControllerBase
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
