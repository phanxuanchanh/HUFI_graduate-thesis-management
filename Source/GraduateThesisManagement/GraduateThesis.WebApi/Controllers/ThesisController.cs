using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/thesis")]
    [ApiController]
    public class ThesisController : ApiControllerBase<IThesisRepository, ThesisInput, ThesisOutput, string>
    {
        private IThesisRepository _thesisRepository;

        public ThesisController(IRepository repository) : base(repository.ThesisRepository)
        {
            _thesisRepository = repository.ThesisRepository;
        }

        [Route("do-thesis-register")]
        [HttpPost]
        public async Task<IActionResult> DoThesisRegister(ThesisRegisterInput thesisRegisterInput)
        {
            if (ModelState.IsValid)
            {
                DataResponse dataResponse = await _thesisRepository.DoThesisRegisterAsync(thesisRegisterInput);

                return Ok(dataResponse);
            }
            return Ok(new { Status = "DoAgain" });
        }
    }
}
