using GraduateThesis.Repository.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Web.Controllers
{
    [Route("lecturer")]
    public class LecturerController : Controller
    {
        private readonly ILecturerRepository _lecturerRepository;
        public LecturerController(IRepository repository)
        {
            _lecturerRepository = repository.LecturerRepository;
        }
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _lecturerRepository.GetListAsync());
        }
    }
}
