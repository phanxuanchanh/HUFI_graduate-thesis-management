using GraduateThesis.ApplicationCore.AppController;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Administrator.Controllers;

[Area("Administrator")]
[Route("admin/dashboard")]
public class DashboardController : WebControllerBase
{
    [Route("index")]
    public IActionResult Index()
    {
        return View();
    }
}
