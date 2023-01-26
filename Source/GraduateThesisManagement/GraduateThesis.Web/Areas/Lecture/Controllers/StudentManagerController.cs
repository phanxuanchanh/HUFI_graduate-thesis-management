using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.File;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/student-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class StudentManagerController : WebControllerBase<IStudentRepository, StudentInput, StudentOutput, string>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentClassRepository _studentClassRepository;

    public StudentManagerController(IRepository repository)
        :base(repository.StudentRepository)
    {
        _studentRepository = repository.StudentRepository;
        _studentClassRepository = repository.StudentClassRepository;
    }

    protected override async Task LoadSelectListAsync()
    {
        List<StudentClassOutput> students = await _studentClassRepository.GetListAsync(50);
        ViewData["StudentClassSelectList"] = new SelectList(students, "Id", "Name");
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách sinh viên của khoa")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 20, string orderBy = "", string orderOptions = "ASC", string keyword = "")
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết sinh viên của khoa")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới sinh viên của khoa")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới sinh viên của khoa")]
    public override async Task<IActionResult> Create(StudentInput studentInput)
    {
        return await CreateResult(studentInput);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa thông tin sinh viên của khoa")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa thông tin sinh viên của khoa")]
    public override async Task<IActionResult> Edit([Required] string id, StudentInput studentInput)
    {
        return await EditResult(id, studentInput);
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
    [PageName(Name = "Xuất dữ liệu ra khỏi hệ thống")]
    public override async Task<IActionResult> Export()
    {
        return await ExportResult();
    }

    [Route("export")]
    [HttpPost]
    public override async Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        byte[] bytes = await _studentRepository.ExportAsync();
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"student_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }

    [Route("import")]
    [HttpPost]
    [PageName(Name = "Nhập dữ liệu vào hệ thống")]
    public override async Task<IActionResult> Import([Required][FromForm] IFormFile formFile, ImportMetadata importMetadata)
    {
        if (string.IsNullOrEmpty(importMetadata.SheetName))
            importMetadata.SheetName = "Default";

        importMetadata.StartFromRow = 2;
        return await ImportResult(formFile, importMetadata);
    }

    [Route("import")]
    [HttpGet]
    [PageName(Name = "Nhập dữ liệu vào hệ thống")]
    public override async Task<IActionResult> Import()
    {
        return await ImportResult();
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

    [Route("registered-students")]
    [HttpGet]
    [PageName(Name = "Danh sách sinh viên đã đăng ký đề tài")]
    public async Task<IActionResult> GetRegdStudents(int page = 1, int pageSize = 20, string keyword = "")
    {
        Pagination<StudentOutput> pagination = await _studentRepository.GetPgnOfRegdStdntAsync(page, pageSize, keyword);
        StaticPagedList<StudentOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("unregistered-students")]
    [HttpGet]
    [PageName(Name = "Danh sách sinh viên chưa đăng ký đề tài")]
    public async Task<IActionResult> GetUnRegdStudents(int page = 1, int pageSize = 20, string keyword = "")
    {
        Pagination<StudentOutput> pagination = await _studentRepository.GetPgnOfUnRegdStdntAsync(page, pageSize, keyword);
        StaticPagedList<StudentOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("export-registered-students")]
    [HttpGet]
    [PageName(Name = "Xuất sinh viên đã đăng ký khóa luận")]
    public async Task<IActionResult> ExportRegdStudents()
    {
        return await ExportResult();
    }

    [Route("export-registered-students")]
    [HttpPost]
    public async Task<IActionResult> ExportRegdStudents(ExportMetadata exportMetadata)
    {
        byte[] bytes = await _studentRepository.ExportRegdStdntsAsync();
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"registered-student_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }

    [Route("export-unregistered-students")]
    [HttpGet]
    [PageName(Name = "Xuất sinh viên đã đăng ký khóa luận")]
    public async Task<IActionResult> ExportUnRegdStudents()
    {
        return await ExportResult();
    }

    [Route("export-unregistered-students")]
    [HttpPost]
    public async Task<IActionResult> ExportUnRegdStudents(ExportMetadata exportMetadata)
    {
        byte[] bytes = await _studentRepository.ExportUnRegdStdntsAsync();
        ContentDisposition contentDisposition = new ContentDisposition
        {
            FileName = $"unregistered-student_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"
        };

        Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

        return File(bytes, ContentTypeConsts.XLSX);
    }
}
