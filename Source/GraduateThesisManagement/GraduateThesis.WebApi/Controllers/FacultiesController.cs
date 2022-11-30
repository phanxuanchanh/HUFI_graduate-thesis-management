using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/faculties")]
    [ApiController]
    public class FacultiesControler : ApiControllerBase<IFacultiesRepository, FacultiesInput, FacultiesOutput, string>
    {
        public FacultiesControler(IRepository repository) : base(repository.FacultiesRepository)
        {
        }
    }
}

