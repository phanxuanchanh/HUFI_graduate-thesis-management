using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.ViewComponents;

[ViewComponent(Name = "AdminLayout_PagedSearch")]
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
