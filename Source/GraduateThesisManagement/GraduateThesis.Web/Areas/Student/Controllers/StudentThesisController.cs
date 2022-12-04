using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.Controllers
{
    public class StudentThesisController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
