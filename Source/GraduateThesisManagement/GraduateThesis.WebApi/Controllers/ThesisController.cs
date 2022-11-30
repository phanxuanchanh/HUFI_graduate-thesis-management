using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/thesis")]
    [ApiController]
    public class ThesisController : ApiControllerBase<IThesisRepository, ThesisInput, ThesisOutput, string>
    {
        public ThesisController(IRepository repository) : base(repository.ThesisRepository)
        {
        }
    }
}
