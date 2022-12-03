using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ApiControllerBase<IStudentRepository, StudentInput, StudentOutput, string>
    {
        public StudentController(IRepository repository) : base(repository.StudentRepository)
        {
        }
       
    }

}
