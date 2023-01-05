using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IStudentThesisGroupRepository : ISubRepository<StudentThesisGroupInput, StudentThesisGroupOutput, string>
{
    Task<DataResponse> ApprovalStudentThesisGroupAsync(string StudentThesisGroupId);
    Task<DataResponse> RefuseApprovalStudentThesisGroupAsync(string StudentThesisGroupId);
}
