using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/council-member")]
    [ApiController]
    public class CouncilMemberControler : ApiControllerBase<ICouncilMemberRepository, CouncilMemberInput, CouncilMemberOutput, string>
    {
        public CouncilMemberControler(IRepository repository) : base(repository.CouncilMemberRepository)
        {
        }
    }
}

