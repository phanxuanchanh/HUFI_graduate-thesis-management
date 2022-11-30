using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/student-thesis-group")]
    [ApiController]
    public class StudentThesisGroupController : ApiControllerBase<IStudentThesisGroupRepository, StudentThesisGroupInput, StudentThesisGroupOutput, string>
    {
        public StudentThesisGroupController(IRepository repository) : base(repository.StudentThesisGroupRepository)
        {
        }
    }
}
