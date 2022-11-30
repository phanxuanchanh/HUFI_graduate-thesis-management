using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/councilMembers")]
    [ApiController]
    public class CouncilMembersControler : ApiControllerBase<ICouncilMembersRepository, CouncilMembersInput, CouncilMembersOutput, string>
    {
        public CouncilMembersControler(IRepository repository) : base(repository.CouncilMembersRepository)
        {
        }
    }
}

