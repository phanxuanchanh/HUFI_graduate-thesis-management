using GraduateThesis.Common;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using GraduateThesis.RepositoryPatterns;
using MathNet.Numerics.Distributions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements
{
    public class StudentRepository : IStudentRepository
    {
        private HUFI_graduatethesisContext _context;
        private GenericRepository<HUFI_graduatethesisContext, Student, StudentInput, StudentOutput> _genericRepository;

        internal StudentRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HUFI_graduatethesisContext, Student, StudentInput, StudentOutput>(_context, _context.Students);

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
            _genericRepository.Selector = s => new StudentOutput
            {
                Id = s.Id,
                Name = s.Name,
                Phone = s.Phone,
                Address = s.Address,
                Avatar = s.Avatar,
                Birthday = s.Birthday,
                Email = s.Email,
                //StudentClass = new StudentClassOutput
                //{
                //    Id = s.StudentClass.Id,
                //    Name = s.StudentClass.Name,
                //    Description = s.StudentClass.Description,
                //    StudentQuantity = s.StudentClass.StudentQuantity,
                //}
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
            string salt = HashFunctions.GetMD5($"{input.Id}|{input.Name}|{DateTime.Now}");
            string passwordAndSalt = $"{input.Password}>>>{salt}";
            
            input.Password = BCrypt.Net.BCrypt.HashPassword(passwordAndSalt);
            input.sa
            return _genericRepository.Create(input, GenerateUIDOptions.None);
        }

        public async Task<DataResponse<StudentOutput>> CreateAsync(StudentInput input)
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

        public SignInResultModel SignIn(SignInModel signInModel)
        {
            Student student = _context.Students.Find(signInModel.Code);
            if (student == null)
                return new SignInResultModel { Status = SignInStatus.NotFound };

            string passwordAndSalt = $"{signInModel.Password}>>>{student.Password}";

            if (!BCrypt.Net.BCrypt.Verify(passwordAndSalt, student.Password))
                return new SignInResultModel { Status = SignInStatus.WrongPassword };

            return new SignInResultModel { Status = SignInStatus.Success };
        }

        public async Task<SignInResultModel> SignInAsync(SignInModel signInModel)
        {
            Student student = await _context.Students.FindAsync(signInModel.Code);
            if (student == null)
                return new SignInResultModel { Status = SignInStatus.NotFound };

            string passwordAndSalt = $"{signInModel.Password}>>>{student.Password}";

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
