using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisRepository : ISubRepository<ThesisInput, ThesisOutput, string>
{
    Task<DataResponse> RegisterThesisAsync(ThesisRegistrationInput thesisRegistrationInput);
    Task<DataResponse> SubmitThesisAsync(string thesisId, string thesisGroupId);
    Task<DataResponse> ApprovalThesisAsync(string thesisId);
    Task<DataResponse> CheckMaxStudentNumberAsync(string thesisId, int currentStudentNumber);
    Task<List<ThesisOutput>> GetApprovalThesisAsync();
}
