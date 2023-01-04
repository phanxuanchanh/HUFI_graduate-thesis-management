using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IStudentRepository : ISubRepository<StudentInput, StudentOutput, string>, IAccountPattern
{
    Task<StudentThesisOutput> GetStudentThesisAsync(string studentId);
}
