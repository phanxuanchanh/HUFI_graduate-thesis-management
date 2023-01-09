using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents;

[ViewComponent(Name = "StudentLayout_Paged")]
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

