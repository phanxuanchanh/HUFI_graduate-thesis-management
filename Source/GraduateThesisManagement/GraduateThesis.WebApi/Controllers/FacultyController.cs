using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/faculties")]
    [ApiController]
    public class FacultyControler : ApiControllerBase<IFacultyRepository, FacultyInput, FacultyOutput, string>
    {
        public FacultyControler(IRepository repository) : base(repository.FacultyRepository)
        {
        }
    }
}

