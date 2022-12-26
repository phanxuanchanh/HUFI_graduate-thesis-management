using GraduateThesis.Generics;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Controllers
{
    public class Error : WebControllerBase
    {
        public IActionResult ShowError()
        {
            return View();
        }
    }
}
