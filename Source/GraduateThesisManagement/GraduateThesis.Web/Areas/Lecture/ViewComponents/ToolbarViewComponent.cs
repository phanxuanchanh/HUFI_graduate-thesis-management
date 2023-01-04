using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "FacultyStaffLayout_Toolbar")]
public class ToolbarViewComponent : ViewComponent
{
    public ToolbarViewComponent()
    {

    }

    public IViewComponentResult Invoke()
    {
        return View();
    }
}
