using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/committee-member")]
    [ApiController]
    public class CommitteeMemberControler : ApiControllerBase<ICommitteeMemberRepository, CommitteeMemberInput, CommitteeMemberOutput, string>
    {
        public CommitteeMemberControler(IRepository repository) : base(repository.CommitteeMemberRepository)
        {
        }
    }
}

