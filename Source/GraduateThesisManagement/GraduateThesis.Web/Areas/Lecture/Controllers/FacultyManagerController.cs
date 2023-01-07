using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    public class FacultyManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
