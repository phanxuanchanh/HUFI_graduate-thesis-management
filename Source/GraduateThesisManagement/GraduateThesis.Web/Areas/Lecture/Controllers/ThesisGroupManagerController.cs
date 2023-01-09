using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/thesisgroup-manager")]
[WebAuthorize(AccountRole.Lecture)]
[AccountInfo(typeof(FacultyStaffOutput))]
public class ThesisGroupManagerController : WebControllerBase<IThesisGroupRepository, ThesisGroupInput, ThesisGroupOutput, string>
{
    private IThesisGroupRepository _thesisGroupRepository;

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

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới nhóm đề tài")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới nhóm đề tài")]
    public override async Task<IActionResult> Create(ThesisGroupInput input)
    {
        return await CreateResult(input);
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

    [Route("force-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
    }

    [Route("trash")]
    [HttpGet]
    public override async Task<IActionResult> GetTrash(int count = 50)
    {
        return await GetTrashResult(count);
    }

    public override Task<IActionResult> Import(IFormFile formFile)
    {
        throw new NotImplementedException();
    }


    public override Task<IActionResult> Import()
    {
        throw new NotImplementedException();
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách nhóm sinh viên làm khóa luận")]
    [WebAuthorize(AccountRole.Lecture)]
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
