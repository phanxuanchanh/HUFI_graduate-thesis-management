using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/student-class")]
    [ApiController]
    public class StudentClassController : ApiControllerBase<IStudentClassRepository, StudentClassInput, StudentClassOutput, string>
    {
        public StudentClassController(IRepository repository) : base(repository.StudentClassRepository)
        {
        }
    }
}
