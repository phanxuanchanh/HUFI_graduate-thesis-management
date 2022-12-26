using GraduateThesis.Common.Authorization;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GraduateThesis.Web.Areas.Student.ViewComponents
{
    [ViewComponent(Name = "TopNavbar")]
    public class TopNavbarViewComponent : ViewComponent
    {
        public TopNavbarViewComponent()
        {

        }

        [AccountInfo(typeof(StudentOutput))]
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
