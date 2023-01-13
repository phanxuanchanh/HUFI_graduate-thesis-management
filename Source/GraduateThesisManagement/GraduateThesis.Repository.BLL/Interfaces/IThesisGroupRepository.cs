using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisGroupRepository : ISubRepository<ThesisGroupInput, ThesisGroupOutput, string>
{
    Task<List<ThesisGroupOutput>> GetListAsync(string studentId);
    Task<ThesisGroupOutput> GetAsync(string studentId, string thesisId);
    Task<DataResponse> ApprovalStudentThesisGroupAsync(string StudentThesisGroupId);
    Task<DataResponse> RefuseApprovalStudentThesisGroupAsync(string StudentThesisGroupId);
}
