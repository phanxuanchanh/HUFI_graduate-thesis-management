using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

#nullable disable

namespace GraduateThesis.Web.Areas.Administrator.Controllers;

[Area("Administrator")]
[Route("system-manager")]
public class SystemManagerController : WebControllerBase
{
    private IRepository _repository;

    public SystemManagerController(IRepository repository)
    {
        _repository = repository;
    }

    [Route("exec-query")]
    [HttpGet]
    public IActionResult ExecQuery()
    {
        return View();
    }

    [Route("exec-query")]
    [HttpPost]
    public IActionResult ExecQuery(ExecQueryInput execQueryInput)
    {
        if (ModelState.IsValid)
        {
            int affected = _repository.ExecuteNonQuery($"{execQueryInput.Query}");
            return View();
        }

        return View(execQueryInput);
    }

    [Route("command")]
    [HttpGet]
    public IActionResult Command()
    {
        return View();
    }

    [Route("command")]
    [HttpPost]
    public IActionResult Command(IFormCollection form)
    {
        return View();
    }
}
