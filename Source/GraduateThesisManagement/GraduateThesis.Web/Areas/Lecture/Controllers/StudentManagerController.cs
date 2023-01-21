using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/student-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class StudentManagerController : WebControllerBase<IStudentRepository, StudentInput, StudentOutput, string>
{
    private readonly IStudentClassRepository _studentClassRepository;

    public StudentManagerController(IRepository repository)
        :base(repository.StudentRepository)
    {
        _studentClassRepository = repository.StudentClassRepository;
    }

    protected override async Task LoadSelectListAsync()
    {
        List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync(50);
        ViewData["StudentClassSelectList"] = new SelectList(studentClasses, "Id", "Name");
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
    [HttpPost]
    public override async Task<IActionResult> Export()
    {
        return await ExportResult(null!, null!);
    }

    [Route("import")]
    [HttpPost]
    [PageName(Name = "Nhập dữ liệu vào hệ thống")]
    public override async Task<IActionResult> Import([Required][FromForm] IFormFile formFile, ImportMetadata importMetadata)
    {
        if (string.IsNullOrEmpty(importMetadata.SheetName))
            importMetadata.SheetName = "Default";

        importMetadata.StartFromRow = 1;
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
}
