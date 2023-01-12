using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Hash;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class StudentRepository : SubRepository<Student, StudentInput, StudentOutput, string>, IStudentRepository
{
    private HufiGraduateThesisContext _context;

    internal StudentRepository(HufiGraduateThesisContext context)
        : base(context, context.Students)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        _genericRepository.IncludeMany(i => i.StudentClass);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new StudentOutput
        {
            Id = s.Id,
            Name = s.Name,
            Phone = s.Phone,
            Email = s.Email,
            Address = s.Address,
            CreatedAt = s.StudentClass.CreatedAt,
            UpdatedAt = s.StudentClass.UpdatedAt,
            DeletedAt = s.StudentClass.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new StudentOutput
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

    public override DataResponse<StudentOutput> Create(StudentInput input)
    {
        string salt = HashFunctions.GetMD5($"{input.Id}|{input.Name}|{DateTime.Now}");
        Student student = new Student
        {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Email = input.Email,
            Phone = input.Phone,
            Address = input.Address,
            Birthday = input.Birthday,
            StudentClassId = input.StudentClassId,
            Password = BCrypt.Net.BCrypt.HashPassword($"{input.Password}>>>{salt}"),
            Salt = salt,
            CreatedAt = DateTime.Now
        };

        _context.Students.Add(student);
        int affected = _context.SaveChanges();

        if (affected == 0)
            return new DataResponse<StudentOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse<StudentOutput>
        {
            Status = DataResponseStatus.Success,
            Data = new StudentOutput
            {
                Id = input.Id,
                Name = input.Name,
                Email = input.Email,
            }
        };
    }

    public override async Task<DataResponse<StudentOutput>> CreateAsync(StudentInput input)
    {
        string salt = HashFunctions.GetMD5($"{input.Id}|{input.Name}|{DateTime.Now}");
        Student student = new Student
        {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Email = input.Email,
            Phone = input.Phone,
            Address = input.Address,
            Birthday = input.Birthday,
            StudentClassId = input.StudentClassId,
            Password = BCrypt.Net.BCrypt.HashPassword($"{input.Password}>>>{salt}"),
            Salt = salt,
            CreatedAt = DateTime.Now
        };

        await _context.Students.AddAsync(student);
        int affected = await _context.SaveChangesAsync();

        if (affected == 0)
            return new DataResponse<StudentOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse<StudentOutput>
        {
            Status = DataResponseStatus.Success,
            Data = new StudentOutput
            {
                Id = input.Id,
                Name = input.Name,
                Email = input.Email,
            }
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

    public AccountVerificationModel ForgotPassword(ForgotPasswordModel forgotPasswordModel)
    {
        throw new NotImplementedException();
    }

    public Task<AccountVerificationModel> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
    {
        throw new NotImplementedException();
    }

    public async Task<StudentThesisOutput> GetStudentThesisAsync(string studentId)
    {
        StudentThesisGroup studentThesisGroup = await _context.StudentThesisGroupDetails
            .Include(i => i.StudentThesisGroup)
            .Where(s => s.StudentId == studentId).Select(s => new StudentThesisGroup
            {

            }).SingleOrDefaultAsync();

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

    public NewPasswordModel VerifyAccount(AccountVerificationModel accountVerificationModel)
    {
        throw new NotImplementedException();
    }

    public Task<NewPasswordModel> VerifyAccountAsync(AccountVerificationModel accountVerificationModel)
    {
        throw new NotImplementedException();
    }

    public async Task<object> SearchForThesisRegAsync(string keyword)
    {
        List<Student> students = await _context.Theses.Where(t => t.IsDeleted == false)
            .Join(
                _context.StudentThesisGroupDetails.Where(gd => gd.IsCompleted == true),
                thesis => thesis.ThesisGroupId, groupDetail => groupDetail.StudentThesisGroupId,
                (thesis, groupDetail) => new { StudentId = groupDetail.StudentId }
            ).Join(
                _context.Students,
                combined => combined.StudentId,
                student => student.Id,
                (combined, student) => new Student { Id = student.Id, Name = student.Name }
            ).Distinct().ToListAsync();

        return await _context.Students.Include(i => i.StudentClass)
            .Where(s => (s.Id.Contains(keyword) || s.Name.Contains(keyword)) && s.IsDeleted == false)
            .WhereBulkNotContains(students)
            .Take(50).Select(s => new
            {
                Id = s.Id,
                Name = s.Name,
                StudentClass = new
                {
                    Id = s.StudentClass.Id,
                    Name = s.StudentClass.Name
                }
            }).ToListAsync();
    }

    public async Task<object> GetObjAsync(string studentId)
    {
        return await _context.Students.Where(s => s.Id == studentId && s.IsDeleted == false)
            .Select(s => new
            {
                Id = s.Id,
                Name = s.Name,
                StudentClass = new
                {
                    Id = s.StudentClass.Id,
                    Name = s.StudentClass.Name
                }
            }).SingleOrDefaultAsync();
    }
}
