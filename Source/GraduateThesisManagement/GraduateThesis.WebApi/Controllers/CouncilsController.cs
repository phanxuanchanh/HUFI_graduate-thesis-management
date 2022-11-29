using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/councils")]
    [ApiController]
    public class CouncilsControler : ApiControllerBase<ICouncilsRepository, CouncilsInput, CouncilsOutput, string>
    {
        public CouncilsControler(IRepository repository) : base(repository.CouncilsRepository)
        {
        }
    }
}

