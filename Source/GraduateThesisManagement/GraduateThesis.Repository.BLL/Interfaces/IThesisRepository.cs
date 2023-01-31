﻿using GraduateThesis.ApplicationCore.Enums;
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
    Task<DataResponse<string>> CheckThesisAvailAsync(string thesisId);
    Task<DataResponse> AllowedRegistration(string studentId, string thesisId);
    Task<Pagination<ThesisOutput>> GetPgnOfPubldThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfRejdThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfAppdThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfPndgApvlThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfThesesInprAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfFinishedThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPaginationAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfPndgApvlThesesAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfPubldThesesAsync(string lecturerId, int page, int pageSize, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfRejdThesesAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfAppdThesesAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnToAssignSupvAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfAssignedSupvAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnToAssignCLectAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnOfAssignedCLectAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<Pagination<ThesisOutput>> GetPgnToAssignCmteAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword);
    Task<byte[]> ExportAsync();
    Task<byte[]> ExportPndgThesesAsync();
    Task<byte[]> ExportPubldThesesAsync();
    Task<byte[]> ExportRejdThesesAsync();
    Task<byte[]> ExportAppdThesesAsync();
    Task<byte[]> ExportAsync(string lecturerId);
    Task<byte[]> ExportPubldThesesAsync(string lecturerId);
    Task<byte[]> ExportRejdThesesAsync(string lecturerId);
    Task<byte[]> ExportAppdThesesAsync(string lecturerId);
    Task<DataResponse> AssignSupervisorAsync(string thesisId, string lecturerId);
    Task<DataResponse> AssignSupervisorAsync(string thesisId);
    Task<DataResponse> AssignSupervisorsAsync(string[] thesisIds);
    Task<DataResponse> RemoveAssignSupvAsync(string thesisId, string lecturerId);
    Task<DataResponse> AssignCLectAsync(string thesisId, string lecturerId);
    Task<DataResponse> RemoveAssignCLectAsync(string thesisId, string lecturerId);
    Task<DataResponse> PublishThesisAsync(string thesisId);
    Task<DataResponse> PublishThesesAsync(string[] thesisIds);
    Task<DataResponse> StopPubgThesisAsync(string thesisId);
    Task<DataResponse> StopPubgThesesAsync(string[] thesisIds);
}
