using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/dashboard")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class FacultyStaffDashboardController : WebControllerBase
{
    [Route("overview")]
    [HttpGet]
    [PageName(Name = "Trang tổng quan")]
    public IActionResult Index()
    {
        return View();
    }
}
