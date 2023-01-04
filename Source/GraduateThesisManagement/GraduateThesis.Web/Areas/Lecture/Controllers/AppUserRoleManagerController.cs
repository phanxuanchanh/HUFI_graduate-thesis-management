using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/appUserRole-manager")]
public class AppUserRoleManagerController : WebControllerBase
{
    private IAppRoleRepository _appRolesRepository;
    private IFacultyRepository _facultyRepository;
    private IAppUserRoleRepository _appUserRolesRepository;

    public AppUserRoleManagerController(IRepository repository)
    {
        _appRolesRepository = repository.AppRolesRepository;
        _facultyRepository = repository.FacultyRepository;
        _appUserRolesRepository = repository.AppUserRolesRepository;
      
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách giảng viên và chức vụ")]
    public async Task<IActionResult> Index()
    {
            return View();
    }

}
