using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/dashboard")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class FacultyStaffDashboardController : WebControllerBase
{
    private readonly IReportRepository _reportRepository;

    public FacultyStaffDashboardController(IRepository repository)
    {
        _reportRepository = repository.ReportRepository;
    }

    [Route("overview")]
    [HttpGet]
    [PageName(Name = "Trang tổng quan")]
    public async Task<IActionResult> Index()
    {
        ViewData["FStaffAreaStats"] = await _reportRepository.GetFacultyStaffAreaStats();

        return View();
    }
}
