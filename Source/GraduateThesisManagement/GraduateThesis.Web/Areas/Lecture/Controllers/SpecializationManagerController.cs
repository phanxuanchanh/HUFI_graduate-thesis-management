using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.WebAttributes;
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

    public override Task<IActionResult> BatchDelete([Required] string id)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Create()
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Create(SpecializationInput input)
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

    public override Task<IActionResult> Edit([Required] string id, SpecializationInput input)
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

    public override Task<IActionResult> GetTrash(int count = 50)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Import()
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Import([FromForm] IFormFile formFile)
    {
        throw new NotImplementedException();
    }

    public IActionResult Index()
    {
        return View();
    }

    public override Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Restore([Required] string id)
    {
        throw new NotImplementedException();
    }
}
