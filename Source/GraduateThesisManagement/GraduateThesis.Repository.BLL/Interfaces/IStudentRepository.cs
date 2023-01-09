using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IStudentRepository : ISubRepository<StudentInput, StudentOutput, string>, IAccountPattern
{
    Task<StudentThesisOutput> GetStudentThesisAsync(string studentId);
    Task<object> SearchForThesisRegAsync(string keyword);
    Task<object> GetObjAsync(string studentId);
}
