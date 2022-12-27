using GraduateThesis.Common;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements
{
    public class StudentRepository : IStudentRepository
    {
        private HufiGraduateThesisContext _context;
        private GenericRepository<HufiGraduateThesisContext, Student, StudentInput, StudentOutput> _genericRepository;

        internal StudentRepository(HufiGraduateThesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HufiGraduateThesisContext, Student, StudentInput, StudentOutput>(_context, _context.Students);

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
            _genericRepository.IncludeMany(i => i.StudentClass);
        }

        public void ConfigureSelectors()
        {
            _genericRepository.PaginationSelector = s => new StudentOutput
            {
                Id = s.Id,
                Name = s.Name,
                Phone = s.Phone,
                Email = s.Email,
                CreatedAt = s.StudentClass.CreatedAt,
                UpdatedAt = s.StudentClass.UpdatedAt,
                DeletedAt = s.StudentClass.DeletedAt
            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new StudentOutput
            {
                Id = s.Id,
                Name = s.Name,
                Phone = s.Phone,
                Email = s.Email,
                Address = s.Address,
                Avatar = s.Avatar,
                Birthday = s.Birthday,
                Description = s.Description,
                StudentClass = new StudentClassOutput
                {
                    Id = s.StudentClass.Id,
                    Name = s.StudentClass.Name,
                    Description = s.StudentClass.Description,
                    CreatedAt = s.StudentClass.CreatedAt,
                    UpdatedAt = s.StudentClass.UpdatedAt,
                    DeletedAt = s.StudentClass.DeletedAt
                },
                CreatedAt = s.StudentClass.CreatedAt,
                UpdatedAt = s.StudentClass.UpdatedAt,
                DeletedAt = s.StudentClass.DeletedAt
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

        public DataResponse<StudentOutput> Create(StudentInput input)
        {
            Student student = _genericRepository.ToEntity(input);
            student.Salt = HashFunctions.GetMD5($"{input.Id}|{input.Name}|{DateTime.Now}");
            student.Password = BCrypt.Net.BCrypt.HashPassword($"{input.Password}>>>{student.Salt}");

            _context.Students.Add(student);
            int affected = _context.SaveChanges();

            if (affected == 0)
                return new DataResponse<StudentOutput> { Status = DataResponseStatus.Failed };

            return new DataResponse<StudentOutput>
            {
                Status = DataResponseStatus.Success,
                Data = _genericRepository.ToOutput(student)
            };
        }

        public async Task<DataResponse<StudentOutput>> CreateAsync(StudentInput input)
        {
            Student student = _genericRepository.ToEntity(input);
            student.Salt = HashFunctions.GetMD5($"{input.Id}|{input.Name}|{DateTime.Now}");
            student.Password = BCrypt.Net.BCrypt.HashPassword($"{input.Password}>>>{student.Salt}");

            await _context.Students.AddAsync(student);
            int affected = await _context.SaveChangesAsync();

            if (affected == 0)
                return new DataResponse<StudentOutput> { Status = DataResponseStatus.Failed };

            return new DataResponse<StudentOutput>
            {
                Status = DataResponseStatus.Success,
                Data = _genericRepository.ToOutput(student)
            };
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

        public StudentOutput Get(string id)
        {
            return _genericRepository.GetById(id);
        }

        public Task<StudentOutput> GetAsync(string id)
        {
            return _genericRepository.GetByIdAsync(id);
        }

        public List<StudentOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public async Task<List<StudentOutput>> GetListAsync(int count = 200)
        {
            return await _genericRepository.GetListAsync(count);
        }

        public Pagination<StudentOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<StudentOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<StudentThesisOutput> GetStudentThesisAsync(string studentId)
        {
            //ThesisOutput thesisOutput = await _genericRepository.GetAsync("Id", thesisId);
            //if (thesisOutput == null)
            //    return null;

            //StudentThesisGroupOutput studentThesisGroup = thesisOutput.StudentThesisGroup;

            List<StudentOutput> students = await _context.StudentThesisGroupDetails
                .Where(s => s.StudentId == studentId).Include(i => i.Student)
                .Select(s => new StudentOutput
                {
                    Id = s.Student.Id,
                    Name = s.Student.Name
                }).ToListAsync();

            return new StudentThesisOutput
            {
                //Thesis = thesisOutput,
                //StudentThesisGroup = studentThesisGroup,
                Students = students
            };
        }

        public DataResponse ImportFromSpreadsheet(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse> ImportFromSpreadsheetAsync(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            throw new NotImplementedException();
        }

        public SignInResultModel SignIn(SignInModel signInModel)
        {
            Student student = _context.Students.Find(signInModel.Code);
            if (student == null)
                return new SignInResultModel { Status = SignInStatus.NotFound };

            string passwordAndSalt = $"{signInModel.Password}>>>{student.Salt}";

            if (!BCrypt.Net.BCrypt.Verify(passwordAndSalt, student.Password))
                return new SignInResultModel { Status = SignInStatus.WrongPassword };

            return new SignInResultModel { Status = SignInStatus.Success };
        }

        public async Task<SignInResultModel> SignInAsync(SignInModel signInModel)
        {
            Student student = await _context.Students.FindAsync(signInModel.Code);
            if (student == null)
                return new SignInResultModel { Status = SignInStatus.NotFound };

            string passwordAndSalt = $"{signInModel.Password}>>>{student.Salt}";

            if (!BCrypt.Net.BCrypt.Verify(passwordAndSalt, student.Password))
                return new SignInResultModel { Status = SignInStatus.WrongPassword };

            return new SignInResultModel { Status = SignInStatus.Success };
        }

        public DataResponse<StudentOutput> Update(string id, StudentInput input)
        {
            return _genericRepository.Update(id, input);
        }

        public Task<DataResponse<StudentOutput>> UpdateAsync(string id, StudentInput input)
        {
            return _genericRepository.UpdateAsync(id, input);
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
