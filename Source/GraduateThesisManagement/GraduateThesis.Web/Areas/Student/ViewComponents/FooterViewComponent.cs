using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.ViewComponents;

[ViewComponent(Name = "FacultyStaffLayout_Footer")]
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
