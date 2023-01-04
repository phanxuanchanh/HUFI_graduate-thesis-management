using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents
{
    [ViewComponent(Name = "TopNavbar")]
    public class TopNavbarViewComponent : ViewComponent
    {
        public TopNavbarViewComponent()
        {

        }

        [AccountInfo(typeof(StudentOutput))]
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
