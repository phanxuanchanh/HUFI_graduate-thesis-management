using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IStudentRepository : ISubRepository<StudentInput, StudentOutput, string>, IAccountPattern
{
    IEmailService EmailService { get; set; }

    Task<StudentThesisOutput> GetStudentThesisAsync(string studentId);
    Task<object> SearchForThesisRegAsync(string keyword);
    Task<object> GetForThesisRegAsync(string studentId);
}
