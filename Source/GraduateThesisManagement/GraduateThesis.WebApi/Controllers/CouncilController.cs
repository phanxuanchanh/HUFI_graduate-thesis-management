using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/council")]
    [ApiController]
    public class CouncilControler : ApiControllerBase<ICouncilRepository, CouncilInput, CouncilOutput, string>
    {
        public CouncilControler(IRepository repository) : base(repository.CouncilRepository)
        {
        }
    }
}

