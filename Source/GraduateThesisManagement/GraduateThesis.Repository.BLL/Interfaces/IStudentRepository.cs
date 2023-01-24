using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IStudentRepository : ISubRepository<StudentInput, StudentOutput, string>, IAsyncAccountPattern
{
    Task<DataResponse> UpdateProfileAsync(StudentInput input, FileUploadModel avtUploadModel);
    Task<DataResponse> SetDefaultAvatarAsync(string studentId);
    Task<object> SearchForThesisRegAsync(string keyword);
    Task<object> GetForThesisRegAsync(string studentId);
    //Task<Pagination<StudentOutput>> GetPgnOf();
    //Task<Pagination<StudentOutput>> GetPgnOf2();
}
