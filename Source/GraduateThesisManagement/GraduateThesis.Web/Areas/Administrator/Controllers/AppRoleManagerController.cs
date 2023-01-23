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

#nullable disable

namespace GraduateThesis.Web.Areas.Administrator.Controllers;

[Area("Administrator")]
[Route("admin/app-role-manager")]
public class AppRoleManagerController : WebControllerBase<IAppRoleRepository, AppRoleInput, AppRoleOutput, string>
{
    private IAppRoleRepository _appRoleRepository;
    private IAppPageRepository _appPageRepository;
    private IFacultyStaffRepository _facultyStaffRepository;

    public AppRoleManagerController(IRepository repository) 
        : base(repository.AppRolesRepository)
    {
        _appRoleRepository = repository.AppRolesRepository;
        _appPageRepository = repository.AppPageRepository;
        _facultyStaffRepository = repository.FacultyStaffRepository;
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách quyền")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    [Route("trash")]
    [HttpGet]
    [PageName(Name = "Thùng rác")]
    public override async Task<IActionResult> GetTrash(int count = 50)
    {
        return await GetTrashResult(count);
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
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    [Route("restore/{id}")]
    [HttpPost]
    public override async Task<IActionResult> Restore([Required] string id)
    {
        return await RestoreResult(id);
    }

    [Route("force-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
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
    public override Task<IActionResult> Import([FromForm] IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new NotImplementedException();
    }

    [Route("grant-to-user/{roleId}")]
    [HttpGet]
    [PageName(Name = "Gán quyền cho tài khoản")]
    public async Task<IActionResult> GrantToUser(string roleId, int page = 1, int pageSize = 10, string keyword = "")
    {
        AppRoleOutput appRole = await _appRoleRepository.GetAsync(roleId);
        if (appRole == null)
            return RedirectToAction("Index");

        Pagination<FacultyStaffOutput> pagination = await _facultyStaffRepository
            .GetPaginationAsync(page, pageSize, null, OrderOptions.ASC, keyword);

        StaticPagedList<FacultyStaffOutput> pagedList = pagination.ToStaticPagedList();
        
        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;
        ViewData["Role"] = appRole;

        return View();
    }

    [Route("grant-to-user/{roleId}")]
    [HttpPost]
    public async Task<IActionResult> GrantToUser(AppUserRoleInput input)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _appRoleRepository.GrantToUserAsync(input);
            AddTempData(dataResponse);

            return RedirectToAction("GrantToUser", new { roleId = input.RoleId });
        }

        AddTempData(DataResponseStatus.InvalidData);
        return RedirectToAction("GrantToUser", new { roleId = input.RoleId });
    }

    [Route("revoke-from-user/{roleId}")]
    [HttpGet]
    [PageName(Name = "Thu hồi quyền của tài khoản")]
    public async Task<IActionResult> RevokeFromUser(string roleId, int page = 1, int pageSize = 10, string keyword = "")
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

    [Route("revoke-from-user/{roleId}")]
    [HttpPost]
    public async Task<IActionResult> RevokeFromUser(AppUserRoleInput input)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _appRoleRepository.RevokeFromUserAsync(input);
            AddTempData(dataResponse);

            return RedirectToAction("RevokeFromUser", new { roleId = input.RoleId });
        }

        AddTempData(DataResponseStatus.InvalidData);
        return RedirectToAction("RevokeFormUser", new { roleId = input.RoleId });
    }

    [Route("grant-to-page/{roleId}")]
    [HttpGet]
    [PageName(Name = "Gán quyền cho trang")]
    public async Task<IActionResult> GrantToPage(string roleId, int page = 1, int pageSize = 10, string keyword = "")
    {
        AppRoleOutput appRole = await _appRoleRepository.GetAsync(roleId);
        if (appRole == null)
            return RedirectToAction("Index");

        Pagination<AppPageOutput> pagination = await _appPageRepository
            .GetPaginationAsync(page, pageSize, null, OrderOptions.ASC, keyword);

        StaticPagedList<AppPageOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;
        ViewData["Role"] = appRole;

        return View();
    }

    [Route("grant-to-page/{roleId}")]
    [HttpPost]
    public async Task<IActionResult> GrantToPage(AppRoleMappingInput input)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _appRoleRepository.GrantToPageAsync(input);
            AddTempData(dataResponse);

            return RedirectToAction("GrantToPage", new { roleId = input.RoleId });
        }

        AddTempData(DataResponseStatus.InvalidData);
        return RedirectToAction("GrantToPage", new { roleId = input.RoleId });
    }

    [Route("revoke-from-page/{roleId}")]
    [HttpGet]
    [PageName(Name = "Thu hồi quyền của trang")]
    public async Task<IActionResult> RevokeFromPage(string roleId, int page = 1, int pageSize = 10, string keyword = "")
    {
        AppRoleOutput appRole = await _appRoleRepository.GetAsync(roleId);
        if (appRole == null)
            return RedirectToAction("Index");

        Pagination<AppPageOutput> pagination = await _appPageRepository
            .GetPgnHasRoleIdAsync(roleId, page, pageSize, keyword);

        StaticPagedList<AppPageOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;
        ViewData["Role"] = appRole;

        return View();
    }

    [Route("revoke-from-page/{roleId}")]
    [HttpPost]
    public async Task<IActionResult> RevokeFromPage(AppRoleMappingInput input)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _appRoleRepository.RevokeFromPageAsync(input);
            AddTempData(dataResponse);

            return RedirectToAction("RevokeFromPage", new { roleId = input.RoleId });
        }

        AddTempData(DataResponseStatus.InvalidData);
        return RedirectToAction("RevokeFromPage", new { roleId = input.RoleId });
    }

    [NonAction]
    public override Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        throw new NotImplementedException();
    }
}
