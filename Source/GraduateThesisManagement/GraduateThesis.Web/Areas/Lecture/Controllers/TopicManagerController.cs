using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.WebAttributes;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/topic-manager")]
[WebAuthorize("")]
public class TopicManagerController : WebControllerBase<ITopicRepository, TopicInput, TopicOutput, string>
{
    private readonly ITopicRepository _topicRepository;

    public TopicManagerController(IRepository repository) : base(repository.TopicRepository)
    {
        _topicRepository = repository.TopicRepository;
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách các chủ đề khóa luận")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới chủ đề khóa luận")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết chủ đề khóa luận")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới chủ đề khóa luận")]
    public override async Task<IActionResult> Create(TopicInput input)
    {
        return await CreateResult(input);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa chủ đề khóa luận")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa chủ đề khóa luận")]
    public override async Task<IActionResult> Edit([Required] string id, TopicInput input)
    {
        return await EditResult(id, input);
    }

    [Route("batch-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    [Route("force-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
    }

    [Route("export")]
    [HttpGet]
    public override async Task<IActionResult> Export()
    {
        RecordFilter recordFilter = new RecordFilter();
        recordFilter.AddFilter();

        ExportMetadata exportMetadata = new ExportMetadata
        {
            FileName = "Topic",
            TypeOptions = ExportTypeOptions.XLSX,
            SheetName = "Default",
            MaxRecordNumber = 1000,
            IncludeProperties = new string[] { "Id", "Name", "Description" }
        };

        return await ExportResult(recordFilter, exportMetadata);
    }

    [Route("import")]
    [HttpGet]
    [PageName(Name = "Nhập dữ liệu vào hệ thống")]
    public override async Task<IActionResult> Import()
    {
        return await ImportResult();
    }

    [Route("import")]
    [HttpPost]
    [PageName(Name = "Nhập dữ liệu vào hệ thống")]
    public override async Task<IActionResult> Import([FromForm] IFormFile formFile)
    {
        ImportMetadata importMetadata = new ImportMetadata
        {
            StartFromRow = 1,
            TypeOptions = ImportTypeOptions.XLSX
        };

        return await ImportResult(formFile, importMetadata);
    }

    [Route("trash")]
    [HttpGet]
    [PageName(Name = "Thùng rác")]
    public override async Task<IActionResult> GetTrash(int count = 50)
    {
        return await GetTrashResult(count);
    }

    [Route("restore/{id}")]
    [HttpPost]
    public override async Task<IActionResult> Restore([Required] string id)
    {
        return await RestoreResult(id);
    }
}
