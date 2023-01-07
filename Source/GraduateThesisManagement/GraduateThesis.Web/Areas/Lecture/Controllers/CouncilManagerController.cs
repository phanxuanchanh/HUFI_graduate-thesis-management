using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    public class CouncilManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
