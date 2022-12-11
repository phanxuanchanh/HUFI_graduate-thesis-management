using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents
{
    [ViewComponent(Name = "Sidebar")]
    public class SidebarViewComponent : ViewComponent
    {
        public SidebarViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
