using GraduateThesis.Generics;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.WebApi.Controllers
{
    [Route("api/studentThesisGroups")]
    [ApiController]
    public class StudentThesisGroupsController : ApiControllerBase<IStudentThesisGroupsRepository, StudentThesisGroupsInput, StudentThesisGroupsOutput, string>
    {
        public StudentThesisGroupsController(IRepository repository) : base(repository.StudentThesisGroupsRepository)
        {
        }
    }
}
