using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IStudentRepository : ISubRepository<StudentInput, StudentOutput, string>, IAsyncAccountPattern
{
    Task<DataResponse> UpdateProfileAsync(StudentInput input, FileUploadModel avtUploadModel);
    Task<DataResponse> SetDefaultAvatarAsync(string studentId);
    Task<StudentThesisOutput> GetStudentThesisAsync(string studentId);
    Task<object> SearchForThesisRegAsync(string keyword);
    Task<object> GetForThesisRegAsync(string studentId);
}
