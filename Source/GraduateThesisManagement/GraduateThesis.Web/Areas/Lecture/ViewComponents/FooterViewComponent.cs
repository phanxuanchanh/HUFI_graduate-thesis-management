using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "StudentLayout_Footer")]
public class FooterViewComponent : ViewComponent
{
    public FooterViewComponent()
    {

    }

    public IViewComponentResult Invoke()
    {
        return View();
    }
}
