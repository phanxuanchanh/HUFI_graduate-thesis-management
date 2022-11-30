using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/theses")]
    [ApiController]
    public class ThesesController : ApiControllerBase<IThesesRepository, ThesesInput, ThesesOutput, string>
    {
        public ThesesController(IRepository repository) : base(repository.ThesesRepository)
        {
        }
    }
}
