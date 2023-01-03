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
public class ThesisGroupManagerController : WebControllerBase<IStudentThesisGroupRepository, StudentThesisGroupInput, StudentThesisGroupOutput, string>
{
    private IStudentThesisGroupRepository _studentThesisGroupRepository;

    public ThesisGroupManagerController(IRepository repository)
        :base(repository.StudentThesisGroupRepository)
        
    {
        _studentThesisGroupRepository = repository.StudentThesisGroupRepository;
       
    }

    public override Task<IActionResult> BatchDelete([Required] string id)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Create()
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Create(StudentThesisGroupInput input)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Details([Required] string id)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Edit([Required] string id)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Edit([Required] string id, StudentThesisGroupInput input)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Export()
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> ForceDelete([Required] string id)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Import(IFormFile formFile)
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
}
