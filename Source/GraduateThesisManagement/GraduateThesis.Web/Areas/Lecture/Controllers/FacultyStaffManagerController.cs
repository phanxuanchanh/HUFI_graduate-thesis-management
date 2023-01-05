using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/facutlystaff-manager")]
[WebAuthorize(AccountRole.Lecture)]
[AccountInfo(typeof(FacultyStaffOutput))]
public class FacultyStaffManagerController : WebControllerBase<IFacultyStaffRepository, FacultyStaffInput, FacultyStaffOutput, string>
{
    public string PageName { get; set; } = "Quản lý giảng viên";

    private IFacultyStaffRepository _facultyStaffRepository;
    private IFacultyRepository _facultyRepository;
    private IAppRoleRepository _appRolesRepository;

    public FacultyStaffManagerController(IRepository repository)
        :base(repository.FacultyStaffRepository)
    {
        _facultyStaffRepository = repository.FacultyStaffRepository;
        _facultyRepository = repository.FacultyRepository;
        _appRolesRepository = repository.AppRolesRepository;
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách giảng viên")]
    [WebAuthorize(AccountRole.Lecture)]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = null!, string orderOptions = "ASC", string keyword = null!)
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa giảng viên")]
    [WebAuthorize(AccountRole.Lecture)]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới giảng viên")]
    [WebAuthorize(AccountRole.Lecture)]
    public override async Task<IActionResult> Create()
    {
        Func<Task> dependency = async () =>
        {
            List<FacultyOutput> faculties = await _facultyRepository.GetListAsync(50);
            ViewData["FacultyList"] = new SelectList(faculties, "Id", "Name");

            List<AppRoleOutput> facultyStaffRoles = await _appRolesRepository.GetListAsync(50);
            ViewData["facultyStaffRolesList"] = new SelectList(facultyStaffRoles, "Id", "Name");
        };

        return await CreateResult(dependency);
    }

    [Route("create")]
    [HttpPost]
    [WebAuthorize(AccountRole.Lecture)]
    public override async Task<IActionResult> Create(FacultyStaffInput facultyStaffInput)
    {
        Func<Task> dependency = async () =>
        {
            List<FacultyOutput> faculties = await _facultyRepository.GetListAsync(50);
            ViewData["FacultyList"] = new SelectList(faculties, "Id", "Name");

            List<AppRoleOutput> facultyStaffRoles = await _appRolesRepository.GetListAsync(50);
            ViewData["facultyStaffRolesList"] = new SelectList(facultyStaffRoles, "Id", "Name");
        };

        return await CreateResult(facultyStaffInput, dependency);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa giảng viên")]
    [WebAuthorize(AccountRole.Lecture)]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        Func<Task> dependency = async () =>
        {
            List<FacultyOutput> faculties = await _facultyRepository.GetListAsync(50);
            ViewData["FacultyList"] = new SelectList(faculties, "Id", "Name");

            List<AppRoleOutput> facultyStaffRoles = await _appRolesRepository.GetListAsync(50);
            ViewData["facultyStaffRolesList"] = new SelectList(facultyStaffRoles, "Id", "Name");
        };

        return await EditResult(id, dependency);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [WebAuthorize(AccountRole.Lecture)]
    public override async Task<IActionResult> Edit([Required] string id, FacultyStaffInput facultyStaffInput)
    {
        Func<Task> dependency = async () =>
        {
            List<FacultyOutput> faculties = await _facultyRepository.GetListAsync(50);
            ViewData["FacultyList"] = new SelectList(faculties, "Id", "Name");

            List<AppRoleOutput> facultyStaffRoles = await _appRolesRepository.GetListAsync(50);
            ViewData["facultyStaffRolesList"] = new SelectList(facultyStaffRoles, "Id", "Name");
        };

        return await EditResult(id, facultyStaffInput);
    }

    [Route("delete/{id}")]
    [HttpPost]
    [WebAuthorize(AccountRole.Lecture)]
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
    }

    public override async Task<IActionResult> Export()
    {
        return await ExportResult(null!, null!);
    }

    public override async Task<IActionResult> Import(IFormFile formFile)
    {
        return await ImportResult(formFile, new ImportMetadata());
    }

    public override Task<IActionResult> Import()
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> GetTrash(int count = 50)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Restore([Required] string id)
    {
        throw new NotImplementedException();
    }
}
