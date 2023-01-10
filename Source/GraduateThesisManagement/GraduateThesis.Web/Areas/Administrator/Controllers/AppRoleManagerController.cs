using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Administrator.Controllers;

[Area("Administrator")]
[Route("admin/app-role-manager")]
public class AppRoleManagerController : WebControllerBase<IAppRoleRepository, AppRoleInput, AppRoleOutput, string>
{
    private IAppRoleRepository _appRoleRepository;

    public AppRoleManagerController(IRepository repository) 
        : base(repository.AppRolesRepository)
    {
        _appRoleRepository = repository.AppRolesRepository;
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
    public override Task<IActionResult> Create()
    {
        throw new NotImplementedException();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới quyền")]
    public override Task<IActionResult> Create(AppRoleInput input)
    {
        throw new NotImplementedException();
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
}
