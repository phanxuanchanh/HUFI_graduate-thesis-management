using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents
{
    [ViewComponent(Name = "BottomNavbar")]
    public class BottomNavbarViewComponent : ViewComponent
    {
        public BottomNavbarViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
