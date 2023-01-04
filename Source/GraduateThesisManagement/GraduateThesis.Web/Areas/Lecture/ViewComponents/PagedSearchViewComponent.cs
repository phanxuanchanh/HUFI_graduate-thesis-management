using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "FacultyStaffLayout_PagedSearch")]
public class PagedSearchViewComponent : ViewComponent
{
    public PagedSearchViewComponent()
    {

    }

    public IViewComponentResult Invoke()
    {
        return View();
    }
}
