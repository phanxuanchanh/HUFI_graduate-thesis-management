using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/thesis-group-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class ThesisGroupManagerController : WebControllerBase<IThesisGroupRepository, ThesisGroupInput, ThesisGroupOutput, string>
{
    private readonly IThesisGroupRepository _thesisGroupRepository;

    public ThesisGroupManagerController(IRepository repository)
        :base(repository.ThesisGroupRepository)
    {
        _thesisGroupRepository = repository.ThesisGroupRepository; 
    }

    [Route("batch-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    [NonAction]
    public override Task<IActionResult> Create()
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override Task<IActionResult> Create(ThesisGroupInput input)
    {
        throw new NotImplementedException();
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết nhóm đề tài")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa nhóm đề tài")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa nhóm đề tài")]
    public override Task<IActionResult> Edit([Required] string id, ThesisGroupInput input)
    {
        return EditResult(id, input);
    }

    [Route("export")]
    public override Task<IActionResult> Export()
    {
        throw new NotImplementedException();
    }

    [Route("export")]
    [HttpPost]
    public override Task<IActionResult> Export(ExportMetadata exportMetadata)
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
    public override Task<IActionResult> Import([Required][FromForm] IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override Task<IActionResult> Import()
    {
        throw new NotImplementedException();
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách nhóm sinh viên làm khóa luận")]
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
