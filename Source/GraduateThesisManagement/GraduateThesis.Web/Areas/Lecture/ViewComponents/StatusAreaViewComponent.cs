using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents
{
    [ViewComponent(Name = "StatusArea")]
    public class StatusAreaViewComponent : ViewComponent
    {
        public StatusAreaViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
