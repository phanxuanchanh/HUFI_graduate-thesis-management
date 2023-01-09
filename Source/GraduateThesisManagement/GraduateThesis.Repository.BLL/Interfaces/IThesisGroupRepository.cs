using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisGroupRepository : ISubRepository<ThesisGroupInput, ThesisGroupOutput, string>
{
    
    Task<DataResponse> ApprovalStudentThesisGroupAsync(string StudentThesisGroupId);
    Task<DataResponse> RefuseApprovalStudentThesisGroupAsync(string StudentThesisGroupId);
}
