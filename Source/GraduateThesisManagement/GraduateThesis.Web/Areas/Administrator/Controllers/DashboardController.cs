using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
