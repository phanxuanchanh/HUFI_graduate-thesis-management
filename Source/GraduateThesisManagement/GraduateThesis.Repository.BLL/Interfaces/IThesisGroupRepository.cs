using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisGroupRepository : ISubRepository<ThesisGroupInput, ThesisGroupOutput, string>
{
    Task<List<ThesisGroupDtOutput>> GetGrpsByStdntIdAsync(string studentId);
    Task<List<ThesisGroupOutput>> GetListAsync(string studentId);
    Task<ThesisGroupOutput> GetAsync(string studentId, string thesisId);
    Task<DataResponse> JoinToGroupAsync(string studentId, string thesisGroupId);
    Task<DataResponse> DenyFromGroupAsync(string studentId, string thesisGroupId);
}
