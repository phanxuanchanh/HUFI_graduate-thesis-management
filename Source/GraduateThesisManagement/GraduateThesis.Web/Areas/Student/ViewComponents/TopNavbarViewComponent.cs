using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents
{
    [ViewComponent(Name = "TopNavbar")]
    public class TopNavbarViewComponent : ViewComponent
    {
        public TopNavbarViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
