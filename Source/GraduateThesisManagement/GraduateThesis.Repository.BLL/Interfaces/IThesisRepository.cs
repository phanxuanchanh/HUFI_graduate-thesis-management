using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisRepository : IAsyncSubRepository<ThesisInput, ThesisOutput, string>
{
    Task<Pagination<ThesisOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
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
    Task<Pagination<ThesisOutput>> GetPgnOf(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfPublishedThesis(string lecturerId, int page, int pageSize, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfRejectedThesis(string lecturerId, int page, int pageSize, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfApprovedThesis(string lecturerId, int page, int pageSize, string keyword);
    Task<DataResponse> AssignSupervisor(string thesisId, string lectureId);
    Task<DataResponse> AssignSupervisor(string thesisId);
    Task<DataResponse> AssignSupervisors();
    Task<DataResponse> AssignCounterArgument(string thesisId, string lectureId);
    Task<DataResponse> AssignCounterArgument(string thesisId);

}
