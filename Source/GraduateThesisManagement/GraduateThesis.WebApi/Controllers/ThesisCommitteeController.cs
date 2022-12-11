using GraduateThesis.Common.Authorization;
using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/thesis-committee")]
    [ApiController]
    [ApiAuthorize()]
    public class ThesisCommitteeControler : ApiControllerBase<IThesisCommitteeRepository, ThesisCommitteeInput, ThesisCommitteeOutput, string>
    {
        public ThesisCommitteeControler(IRepository repository) : base(repository.ThesisCommitteeRepository)
        {
        }
    }
}

