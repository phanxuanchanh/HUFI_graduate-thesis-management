using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/studentClasses")]
    [ApiController]
    public class StudentClassesController : ApiControllerBase<IStudentClassesRepository, StudentClassesInput, StudentClassesOutput, string>
    {
        public StudentClassesController(IRepository repository) : base(repository.StudentClassesRepository)
        {
        }
    }
}
