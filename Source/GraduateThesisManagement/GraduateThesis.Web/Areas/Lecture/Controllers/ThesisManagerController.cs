using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/thesis-manager")]
    public class ThesisManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
