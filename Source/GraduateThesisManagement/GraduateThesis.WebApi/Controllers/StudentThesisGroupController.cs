using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GraduateThesis.WebApi.Controllers;

[Route("api/student-thesis-group")]
[ApiController]
public class StudentThesisGroupController : ApiControllerBase<IStudentThesisGroupRepository, StudentThesisGroupInput, StudentThesisGroupOutput, string>
{
    public StudentThesisGroupController(IRepository repository) : base(repository.StudentThesisGroupRepository)
    {
    }

    [Route("batch-delete")]
    [HttpDelete]
    public override Task<IActionResult> BatchDelete([Required] string id)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Create(StudentThesisGroupInput input)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> ForceDelete([Required] string id)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> GetDetails([Required] string id)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> GetPagination(Pagination<StudentThesisGroupOutput> pagination)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Import(IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Update([Required] string id, StudentThesisGroupInput input)
    {
        throw new System.NotImplementedException();
    }
}
