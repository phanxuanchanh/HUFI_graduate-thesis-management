using GraduateThesis.Repository.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Web.Controllers
{
    [Route("thesis")]
    public class ThesisController : Controller
    {
        
        private readonly IThesisRepository _thesisRepository;
        public ThesisController(IRepository repository)
        {
            _thesisRepository = repository.ThesisRepository;
        }
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _thesisRepository.GetListAsync());
        }

    }
}
