using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisCommitteeRepository : IAsyncSubRepository<ThesisCommitteeInput, ThesisCommitteeOutput, string>
{
    Task<DataResponse> AddMemberAsync(CommitteeMemberInput input);
    Task<DataResponse> DeleteMemberAsync(string committeeId, string memberId);
}
