using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents;

[ViewComponent(Name = "StudentLayout_PagedSearch")]
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
