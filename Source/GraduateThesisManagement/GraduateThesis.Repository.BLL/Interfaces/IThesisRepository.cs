using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisRepository : ISubRepository<ThesisInput, ThesisOutput, string>
{
    Task<DataResponse> RegisterThesisAsync(ThesisRegistrationInput thesisRegistrationInput);
    Task<DataResponse> SubmitThesisAsync(string thesisId, string thesisGroupId);
    Task<DataResponse> ApproveThesisAsync(ThesisApprovalInput approvalInput); 
    Task<DataResponse> RejectThesisAsync(ThesisApprovalInput approvalInput); 
    Task<DataResponse> CheckMaxStudentNumberAsync(string thesisId, int currentStudentNumber);
    Task<DataResponse<string>> CheckThesisAvailable(string thesisId);
    Task<DataResponse> AllowedRegistration(string studentId, string thesisId);
    Task<Pagination<ThesisOutput>> GetPgnOfPublishedThesis(int page, int pageSize, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfRejectedThesis(int page, int pageSize, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfApprovedThesis(int page, int pageSize, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfPublishedThesis(string lecturerId, int page, int pageSize, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfRejectedThesis(string lecturerId, int page, int pageSize, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfApprovedThesis(string lecturerId, int page, int pageSize, string keyword);
}
