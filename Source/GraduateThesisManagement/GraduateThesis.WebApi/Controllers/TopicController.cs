using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/topic")]
    [ApiController]
    public class TopicController : ApiControllerBase<ITopicRepository, TopicInput, TopicOutput, string>
    {
        public TopicController(IRepository repository) : base(repository.TopicRepository)
        {
        }
    }
}
