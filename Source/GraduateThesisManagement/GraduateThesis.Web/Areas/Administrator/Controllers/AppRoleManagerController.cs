using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Administrator.Controllers;

[Area("Administrator")]
[Route("admin/app-role-manager")]
public class AppRoleManagerController : WebControllerBase<IAppRoleRepository, AppRoleInput, AppRoleOutput, string>
{
    private IAppRoleRepository _appRoleRepository;
    private IFacultyStaffRepository _facultyStaffRepository;

    public AppRoleManagerController(IRepository repository) 
        : base(repository.AppRolesRepository)
    {
        _appRoleRepository = repository.AppRolesRepository;
        _facultyStaffRepository = repository.FacultyStaffRepository;
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách quyền")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    public override Task<IActionResult> GetTrash(int count = 50)
    {
        throw new NotImplementedException();
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết quyền")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới quyền")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới quyền")]
    public override async Task<IActionResult> Create(AppRoleInput input)
    {
        return await CreateResult(input);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa quyền")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa quyền")]
    public override async Task<IActionResult> Edit([Required] string id, AppRoleInput input)
    {
        return await EditResult(id, input);
    }

    [Route("batch-delete/{id}")]
    [HttpPost]
    public override Task<IActionResult> BatchDelete([Required] string id)
    {
        throw new NotImplementedException();
    }

    [Route("restore/{id}")]
    [HttpPost]
    public override Task<IActionResult> Restore([Required] string id)
    {
        throw new NotImplementedException();
    }

    [Route("force-delete/{id}")]
    [HttpPost]
    public override Task<IActionResult> ForceDelete([Required] string id)
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override Task<IActionResult> Export()
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override Task<IActionResult> Import()
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override Task<IActionResult> Import([FromForm] IFormFile formFile)
    {
        throw new NotImplementedException();
    }

    [Route("grant/{roleId}")]
    [HttpGet]
    [PageName(Name = "Gán quyền cho tài khoản")]
    public async Task<IActionResult> Grant(string roleId, int page = 1, int pageSize = 10, string keyword = "")
    {
        AppRoleOutput appRole = await _appRoleRepository.GetAsync(roleId);
        if (appRole == null)
            return RedirectToAction("Index");

        Pagination<FacultyStaffOutput> pagination = await _facultyStaffRepository
            .GetPgnHasNotRoleIdAsync(roleId, page, pageSize, keyword);

        StaticPagedList<FacultyStaffOutput> pagedList = pagination.ToStaticPagedList();
        
        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;
        ViewData["Role"] = appRole;

        return View();
    }

    [Route("grant/{roleId}")]
    [HttpPost]
    public async Task<IActionResult> Grant(AppUserRoleInput appUserRoleInput)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _appRoleRepository.GrantAsync(appUserRoleInput);

            AddTempData(dataResponse);
            return RedirectToAction("Grant", new { roleId = appUserRoleInput.RoleId });
        }

        AddTempData(DataResponseStatus.InvalidData);
        return RedirectToAction("Grant", new { roleId = appUserRoleInput.RoleId });
    }

    [Route("revoke/{roleId}")]
    [HttpGet]
    [PageName(Name = "Thu hồi quyền của tài khoản")]
    public async Task<IActionResult> Revoke(string roleId, int page = 1, int pageSize = 10, string keyword = "")
    {
        AppRoleOutput appRole = await _appRoleRepository.GetAsync(roleId);
        if (appRole == null)
            return RedirectToAction("Index");

        Pagination<FacultyStaffOutput> pagination = await _facultyStaffRepository
            .GetPgnHasRoleIdAsync(roleId, page, pageSize, keyword);

        StaticPagedList<FacultyStaffOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;
        ViewData["Role"] = appRole;

        return View();
    }

    [Route("revoke/{roleId}")]
    [HttpPost]
    public async Task<IActionResult> Revoke(AppUserRoleInput appUserRoleInput)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _appRoleRepository.RevokeAsync(appUserRoleInput);

            AddTempData(dataResponse);
            return RedirectToAction("Revoke", new { roleId = appUserRoleInput.RoleId });
        }

        AddTempData(DataResponseStatus.InvalidData);
        return RedirectToAction("Revoke", new { roleId = appUserRoleInput.RoleId });
    }
}
