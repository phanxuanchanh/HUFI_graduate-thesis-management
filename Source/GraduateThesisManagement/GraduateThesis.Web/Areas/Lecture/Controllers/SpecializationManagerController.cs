using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/specialization-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class SpecializationManagerController : WebControllerBase<ISpecializationRepository, SpecializationInput, SpecializationOutput, string>
{
    public SpecializationManagerController(IRepository repository) 
        : base(repository.SpecializationRepository)
    {

    }

    [Route("batch-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới chuyên ngành")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới chuyên ngành")]
    public override async Task<IActionResult> Create(SpecializationInput input)
    {
        return await CreateResult(input);
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết chuyên ngành")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa chuyên ngành")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa chuyên ngành")]
    public override async Task<IActionResult> Edit([Required] string id, SpecializationInput input)
    {
        return await EditResult(id, input);
    }

    [NonAction]
    public override Task<IActionResult> Export()
    {
        throw new NotImplementedException();
    }

    [Route("force-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
    }


    [Route("trash")]
    [HttpGet]
    [PageName(Name = "Thùng rác")]
    public override async Task<IActionResult> GetTrash(int count = 50)
    {
        return await GetTrashResult(count);
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

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách các chuyên ngành")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    [Route("restore/{id}")]
    [HttpPost]
    public override async Task<IActionResult> Restore([Required] string id)
    {
        return await RestoreResult(id);
    }
}
