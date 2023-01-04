using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents
{
    [ViewComponent(Name = "FacultyStaffLayout_Paged")]
    public class PagedViewComponent : ViewComponent
    {
        public PagedViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
