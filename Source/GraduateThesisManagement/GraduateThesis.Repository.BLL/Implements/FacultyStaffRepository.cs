using GraduateThesis.Common;
using GraduateThesis.ExtensionMethods;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements
{
    public class FacultyStaffRepository : IFacultyStaffRepository
    {
        private HufiGraduateThesisContext _context;
        private GenericRepository<HufiGraduateThesisContext, FacultyStaff, FacultyStaffInput, FacultyStaffOutput> _genericRepository;

        internal FacultyStaffRepository(HufiGraduateThesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HufiGraduateThesisContext, FacultyStaff, FacultyStaffInput, FacultyStaffOutput>(context, context.FacultyStaffs);

            ConfigureIncludes();
            ConfigureSelectors();
        }

        public DataResponse BatchDelete(string id)
        {
            return _genericRepository.BatchDelete(id);
        }

        public async Task<DataResponse> BatchDeleteAsync(string id)
        {
            return await _genericRepository.BatchDeleteAsync(id);
        }

        public void ConfigureIncludes()
        {
            _genericRepository.IncludeMany(i => i.FacultyRole);
        }

        public void ConfigureSelectors()
        {
            _genericRepository.PaginationSelector = s => new FacultyStaffOutput
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email,
                Phone = s.Phone
            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new FacultyStaffOutput
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email,
                Phone = s.Phone,
            };
        }

        public int Count()
        {
            return _genericRepository.Count();
        }

        public async Task<int> CountAsync()
        {
            return await _genericRepository.CountAsync();
        }

        public DataResponse<FacultyStaffOutput> Create(FacultyStaffInput input)
        {
            return _genericRepository.Create(input, GenerateUIDOptions.None);
        }

        public async Task<DataResponse<FacultyStaffOutput>> CreateAsync(FacultyStaffInput input)
        {
            return await _genericRepository.CreateAsync(input, GenerateUIDOptions.None);
        }

        public ForgotPasswordModel CreateNewPassword(NewPasswordModel newPasswordModel)
        {
            throw new NotImplementedException();
        }

        public Task<ForgotPasswordModel> CreateNewPasswordAsync(NewPasswordModel newPasswordModel)
        {
            throw new NotImplementedException();
        }

        public IWorkbook ExportToSpreadsheet(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            return _genericRepository.ExportToSpreadsheet(spreadsheetTypeOptions, sheetName, includeProperties);
        }

        public async Task<IWorkbook> ExportToSpreadsheetAsync(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            return await _genericRepository
                .ExportToSpreadsheetAsync(spreadsheetTypeOptions, sheetName, includeProperties);
        }

        public DataResponse ForceDelete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse> ForceDeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public AccountVerificationModel ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            throw new NotImplementedException();
        }

        public Task<AccountVerificationModel> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
        {
            throw new NotImplementedException();
        }

        public FacultyStaffOutput Get(string id)
        {
            return _genericRepository.Get("Id", id);
        }

        public async Task<FacultyStaffOutput> GetAsync(string id)
        {
            return await _genericRepository.GetAsync("Id", id);
        }

        public List<FacultyStaffOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public async Task<List<FacultyStaffOutput>> GetListAsync(int count = 200)
        {
            return await _genericRepository.GetListAsync(count);
        }

        public async Task<List<FacultyStaffOutput>> GetListByRoleIdAsync(string roleId, int count = 200)
        {
            return await _genericRepository
                .GetListByConditionAsync($"{nameof(FacultyStaff.FacultyRoleId)} == {roleId}", count);
        }

        public Pagination<FacultyStaffOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<FacultyStaffOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<FacultyStaffOutput>> GetPaginationByRoleIdAsync(string roleId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository
                .GetPaginationAsync(page, pageSize, orderBy, orderOptions, new string[] { $"{nameof(FacultyStaff.FacultyRoleId)} == {roleId}" }, keyword);
        }

        public DataResponse ImportFromSpreadsheet(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            return _genericRepository.ImportFromSpreadsheet(stream, spreadsheetTypeOptions, sheetName, s =>
            {
                DateTime currentDateTime = DateTime.Now;
                FacultyStaff facultyStaff = new FacultyStaff
                {
                    Id = UID.GetShortUID(),
                    CreatedAt = currentDateTime
                };

                facultyStaff.FacultyId = s.GetCell(1).StringCellValue;
                facultyStaff.FacultyRoleId = s.GetCell(2).StringCellValue;
                facultyStaff.FullName = s.GetCell(3).StringCellValue;
                facultyStaff.Description = s.GetCell(4).StringCellValue;
                facultyStaff.Gender = s.GetCell(5).StringCellValue;
                facultyStaff.Phone = s.GetCell(6).StringCellValue;
                facultyStaff.Address = s.GetCell(7).StringCellValue;
                facultyStaff.Email = s.GetCell(8).StringCellValue;
                facultyStaff.Birthday = s.GetCell(9).DateCellValue;
                facultyStaff.Avatar = s.GetCell(10).StringCellValue;
                facultyStaff.Position = s.GetCell(11).StringCellValue;
                facultyStaff.Degree = s.GetCell(12).StringCellValue;
                facultyStaff.Notes = s.GetCell(13).StringCellValue;
                facultyStaff.Password = s.GetCell(14).StringCellValue;
                facultyStaff.Salt = s.GetCell(15).StringCellValue;

                return facultyStaff;
            });
        }

        public async Task<DataResponse> ImportFromSpreadsheetAsync(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {

            return await _genericRepository.ImportFromSpreadsheetAsync(stream, spreadsheetTypeOptions, sheetName, s =>
            {
                DateTime currentDateTime = DateTime.Now;
                FacultyStaff facultyStaff = new FacultyStaff
                {
                    Id = UID.GetShortUID(),
                    CreatedAt = currentDateTime
                };

                facultyStaff.FacultyId = s.GetCell(1).StringCellValue;
                facultyStaff.FacultyRoleId = s.GetCell(2).StringCellValue;
                facultyStaff.FullName = s.GetCell(3).StringCellValue;
                facultyStaff.Description = s.GetCell(4).StringCellValue;
                facultyStaff.Gender = s.GetCell(5).StringCellValue;
                facultyStaff.Phone = s.GetCell(6).StringCellValue;
                facultyStaff.Address = s.GetCell(7).StringCellValue;
                facultyStaff.Email = s.GetCell(8).StringCellValue;
                facultyStaff.Birthday = s.GetCell(9).DateCellValue;
                facultyStaff.Avatar = s.GetCell(10).StringCellValue;
                facultyStaff.Position = s.GetCell(11).StringCellValue;
                facultyStaff.Degree = s.GetCell(12).StringCellValue;
                facultyStaff.Notes = s.GetCell(13).StringCellValue;
                facultyStaff.Password = s.GetCell(14).StringCellValue;
                facultyStaff.Salt = s.GetCell(15).StringCellValue;

                return facultyStaff;
            });
        }

        public SignInResultModel SignIn(SignInModel signInModel)
        {
            FacultyStaff facultyStaff = _context.FacultyStaffs.Find(signInModel.Code);
            if (facultyStaff == null)
                return new SignInResultModel { Status = SignInStatus.NotFound };

            string passwordAndSalt = $"{signInModel.Password}>>>{facultyStaff.Salt}";

            if (!BCrypt.Net.BCrypt.Verify(passwordAndSalt, facultyStaff.Password))
                return new SignInResultModel { Status = SignInStatus.WrongPassword };

            return new SignInResultModel { Status = SignInStatus.Success };
        }

        public async Task<SignInResultModel> SignInAsync(SignInModel signInModel)
        {
            FacultyStaff facultyStaff = await _context.FacultyStaffs.FindAsync(signInModel.Code);
            if (facultyStaff == null)
                return new SignInResultModel { Status = SignInStatus.NotFound };

            string passwordAndSalt = $"{signInModel.Password}>>>{facultyStaff.Salt}";

            if (!BCrypt.Net.BCrypt.Verify(passwordAndSalt, facultyStaff.Password))
                return new SignInResultModel { Status = SignInStatus.WrongPassword };

            return new SignInResultModel { Status = SignInStatus.Success };
        }

        public DataResponse<FacultyStaffOutput> Update(string id, FacultyStaffInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<FacultyStaffOutput>> UpdateAsync(string id, FacultyStaffInput input)
        {
            throw new NotImplementedException();
        }

        public NewPasswordModel VerifyAccount(AccountVerificationModel accountVerificationModel)
        {
            throw new NotImplementedException();
        }

        public Task<NewPasswordModel> VerifyAccountAsync(AccountVerificationModel accountVerificationModel)
        {
            throw new NotImplementedException();
        }
    }
}
