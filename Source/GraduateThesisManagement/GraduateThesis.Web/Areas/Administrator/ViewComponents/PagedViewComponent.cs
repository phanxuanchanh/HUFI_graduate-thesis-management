using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.ViewComponents;

[ViewComponent(Name = "AdminLayout_Paged")]
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

