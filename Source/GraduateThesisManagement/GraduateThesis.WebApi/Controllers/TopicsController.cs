using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicsController : ApiControllerBase<ITopicsRepository, TopicsInput, TopicsOutput, string>
    {
        public TopicsController(IRepository repository) : base(repository.TopicsRepository)
        {
        }
    }
}
