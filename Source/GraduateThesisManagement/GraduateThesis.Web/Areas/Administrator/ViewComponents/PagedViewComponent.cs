using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.ViewComponents;

[ViewComponent(Name = "AdminLayout_Paged")]
public class PagedViewComponent : ViewComponent
{
    public PagedViewComponent()
    {

    }

    public IViewComponentResult Invoke(string action = "Index", bool enableOrder = true)
    {
        ViewData["Action"] = action;
        ViewData["EnableOrder"] = enableOrder;

        return View();
    }
}

