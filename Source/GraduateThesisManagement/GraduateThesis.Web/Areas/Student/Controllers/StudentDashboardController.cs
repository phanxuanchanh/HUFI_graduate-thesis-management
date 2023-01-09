using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.Controllers
{
    public class StudentDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
