using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Email;
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
[Route("lecture/student-manager")]
[WebAuthorize]
public class StudentManagerController : WebControllerBase<IStudentRepository, StudentInput, StudentOutput, string>
{
    private IStudentRepository _studentRepository;
    private IStudentClassRepository _studentClassRepository;

    public StudentManagerController(IRepository repository, IEmailService emailService)
        :base(repository.StudentRepository)
    {
        _studentRepository = repository.StudentRepository;
        _studentClassRepository = repository.StudentClassRepository;
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách sinh viên của khoa")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
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
        Func<Task> dependency = async () =>
        {
            List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync(50);
            ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");
        };

        return await CreateResult(dependency);
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới sinh viên của khoa")]
    public override async Task<IActionResult> Create(StudentInput studentInput)
    {
        Func<Task> dependency = async () =>
        {
            List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync(50);
            ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");
        };

        return await CreateResult(studentInput, dependency);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa thông tin sinh viên của khoa")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        Func<Task> dependency = async () =>
        {
            List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync(50);
            ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");
        };

        return await EditResult(id, dependency);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa thông tin sinh viên của khoa")]
    public override async Task<IActionResult> Edit([Required] string id, StudentInput studentInput)
    {
        Func<Task> dependency = async () =>
        {
            List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync(50);
            ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");
        };

        return await Edit(id, studentInput);
    }

    [Route("delete/{id}")]
    [HttpPost]
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
