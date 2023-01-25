using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.File;
using GraduateThesis.ApplicationCore.Hash;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ExtensionMethods;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class StudentRepository : SubRepository<Student, StudentInput, StudentOutput, string>, IStudentRepository
{
    private HufiGraduateThesisContext _context;
    private IHostingEnvironment _hostingEnvironment;
    private IEmailService _emailService;
    private IFileManager _fileManager;

    internal StudentRepository(HufiGraduateThesisContext context, IHostingEnvironment hostingEnvironment, IEmailService emailService, IFileManager fileManager)
        : base(context, context.Students)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _emailService = emailService;
        _fileManager = fileManager;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.StudentClass);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new StudentOutput
        {
            Id = s.Id,
            Surname = s.Surname,
            Name = s.Name,
            Address = s.Address,
            StudentClass = new StudentClassOutput
            {
                Id = s.StudentClass.Id,
                Name = s.StudentClass.Name
            },
            CreatedAt = s.StudentClass.CreatedAt,
            UpdatedAt = s.StudentClass.UpdatedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new StudentOutput
        {
            Id = s.Id,
            Surname = s.Surname,
            Name = s.Name,
            Phone = s.Phone,
            Email = s.Email,
            Address = s.Address,
            Avatar = s.Avatar,
            Birthday = s.Birthday,
            Gender = s.Gender,
            Description = s.Description,
            StudentClass = new StudentClassOutput
            {
                Id = s.StudentClass.Id,
                Name = s.StudentClass.Name,
                Description = s.StudentClass.Description,
            },
            CreatedAt = s.StudentClass.CreatedAt,
            UpdatedAt = s.StudentClass.UpdatedAt,
            DeletedAt = s.StudentClass.DeletedAt
        };

        AdvancedImportSelector = s =>
        {
            Student student = new Student();

            student.Id = s[1] as string;
            student.Surname = s[2] as string;
            student.Name = s[3] as string;

            string studentClassId = s[4] as string;
            if(!_context.StudentClasses.Any(s => s.Id == studentClassId))
            {
                _context.StudentClasses.Add(new StudentClass
                {
                    Id = studentClassId,
                    Name = studentClassId,
                    CreatedAt = DateTime.Now
                });

                _context.SaveChanges();
            }

            student.StudentClassId = s[4] as string;
            student.Email = s[5] as string;
            student.Birthday = DateTime.ParseExact(s[6] as string, "dd/MM/yyyy", null);
            student.Password = "default";
            student.Salt = "default";

            return student;
        };
    }

    public override async Task<DataResponse> ImportAsync(Stream stream, ImportMetadata importMetadata)
    {
        return await _genericRepository.ImportAsync(stream, importMetadata, new ImportSelector<Student>
        {
            AdvancedImportSpreadsheet = AdvancedImportSelector
        });
    }

    public override async Task<DataResponse<StudentOutput>> CreateAsync(StudentInput input)
    {
        Student student = new Student
        {
            Id = input.Id,
            Surname = input.Surname,
            Name = input.Name,
            Description = input.Description,
            Email = input.Email,
            Phone = input.Phone,
            Address = input.Address,
            Birthday = input.Birthday,
            StudentClassId = input.StudentClassId,
            Password = "default",
            Salt = "default",
            CreatedAt = DateTime.Now
        };

        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        return new DataResponse<StudentOutput>
        {
            Status = DataResponseStatus.Success,
            Data = new StudentOutput
            {
                Id = input.Id,
                Surname = input.Surname,
                Name = input.Name,
                Email = input.Email,
            }
        };
    }

    public async Task<ForgotPasswordModel> CreateNewPasswordAsync(NewPasswordModel newPasswordModel)
    {
        Student student = await _context.Students
           .Where(s => s.Email == newPasswordModel.Email && s.IsDeleted == false)
           .SingleOrDefaultAsync();

        if (student == null)
            return new ForgotPasswordModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        student.Salt = HashFunctions.GetMD5($"{student.Id}|{student.Name}|{DateTime.Now}");
        student.Password = BCrypt.Net.BCrypt.HashPassword($"{newPasswordModel.Password}>>>{student.Salt}");

        await _context.SaveChangesAsync();

        return new ForgotPasswordModel
        {
            Status = AccountStatus.Success,
            Message = "Đã đổi mật khẩu thành công, hãy thực hiện đăng nhập lại để truy cập vào hệ thống!"
        };
    }

    public async Task<AccountVerificationModel> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
    {
        Student student = await _context.Students
           .Where(f => f.Email == forgotPasswordModel.Email && f.IsDeleted == false)
           .SingleOrDefaultAsync();

        if (student == null)
            return new AccountVerificationModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        Random random = new Random();
        student.VerificationCode = random.NextString(24);
        student.CodeExpTime = DateTime.Now.AddMinutes(5);
        await _context.SaveChangesAsync();

        string mailContent = Resources.EmailResource.account_verification;
        mailContent = mailContent.Replace("@verificationCode", student.VerificationCode);

        _emailService.Send(
            student.Email,
            $"Khôi phục mật khẩu [{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}]",
            mailContent
        );

        return new AccountVerificationModel
        {
            Status = AccountStatus.Success,
            Message = "Thực hiện bước 1 của quá trình lấy lại mật khẩu thành công!",
            Email = forgotPasswordModel.Email
        };
    }

    public async Task<SignInResultModel> SignInAsync(SignInModel signInModel)
    {
        Student student = await _context.Students.FindAsync(signInModel.Code);
        if (student == null)
            return new SignInResultModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        string passwordAndSalt = $"{signInModel.Password}>>>{student.Salt}";

        if (!BCrypt.Net.BCrypt.Verify(passwordAndSalt, student.Password))
            return new SignInResultModel
            {
                Status = AccountStatus.WrongPassword,
                Message = "Mật khẩu không trùng khớp!"
            };

        return new SignInResultModel
        {
            Status = AccountStatus.Success,
            Message = "Đã đăng nhập vào hệ thống thành công!"
        };
    }

    public async Task<NewPasswordModel> VerifyAccountAsync(AccountVerificationModel accountVerificationModel)
    {
        Student student = await _context.Students
            .Where(s => s.Email == accountVerificationModel.Email && s.IsDeleted == false
            ).SingleOrDefaultAsync();

        if (student == null)
            return new NewPasswordModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        if (student.VerificationCode != accountVerificationModel.VerificationCode)
            return new NewPasswordModel
            {
                Status = AccountStatus.Failed,
                Message = "Mã xác thực không trùng khớp!",
                Email = student.Email,
            };

        DateTime currentDatetime = DateTime.Now;
        if (student.CodeExpTime < currentDatetime)
            return new NewPasswordModel
            {
                Status = AccountStatus.Failed,
                Message = "Mã xác nhận đã hết hạn!"
            };

        return new NewPasswordModel
        {
            Status = AccountStatus.Success,
            Message = "Thực hiện bước 2 của quá trình lấy lại mật khẩu thành công!",
        };
    }

    public async Task<object> SearchForThesisRegAsync(string keyword)
    {
        List<Student> students = await _context.Theses.Where(t => t.IsDeleted == false)
            .Join(
                _context.ThesisGroupDetails.Where(gd => gd.IsCompleted == true),
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
                Surname = s.Surname,
                Name = s.Name,
                StudentClass = new
                {
                    Id = s.StudentClass.Id,
                    Name = s.StudentClass.Name
                }
            }).ToListAsync();
    }

    public async Task<object> GetForThesisRegAsync(string studentId)
    {
        return await _context.Students.Where(s => s.Id == studentId && s.IsDeleted == false)
            .Select(s => new
            {
                Id = s.Id,
                Surname = s.Surname,
                Name = s.Name,
                StudentClass = new
                {
                    Id = s.StudentClass.Id,
                    Name = s.StudentClass.Name
                }
            }).SingleOrDefaultAsync();
    }

    public async Task<DataResponse> UpdateProfileAsync(StudentInput input, FileUploadModel avtUploadModel)
    {
        Student student_fromDb = await _context.Students.FindAsync(input.Id);
        if (student_fromDb == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        student_fromDb.Email = input.Email;
        student_fromDb.Phone = input.Phone;
        student_fromDb.Address = input.Address;
        student_fromDb.Birthday = input.Birthday;
        student_fromDb.Gender = input.Gender;

        if (avtUploadModel != null)
        {
            avtUploadModel.FileName = $"student-avatar_{student_fromDb.Id}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{_fileManager.GetExtension(avtUploadModel.ContentType)}";

            _fileManager.SetPath(Path.Combine(_hostingEnvironment.WebRootPath, "avatar", "student"));
            _fileManager.Save(avtUploadModel);

            student_fromDb.Avatar = $"student/{avtUploadModel.FileName}";
        }

        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Cập nhật thông tin cá nhân thành công!"
        };
    }

    public async Task<DataResponse> SetDefaultAvatarAsync(string studentId)
    {
        Student student_fromDb = await _context.Students.FindAsync(studentId);
        if (student_fromDb == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        if (student_fromDb.Gender == "Nam")
            student_fromDb.Avatar = "default-male-profile.png";
        else
            student_fromDb.Avatar = "default-female-profile.png";

        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã đặt ảnh đại diện mặc định thành công!"
        };
    }

    public Task<byte[]> ExportUnRegdStdntAsync(ExportMetadata exportMetadata)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> ExportRegdStdntAsync(ExportMetadata exportMetadata)
    {
        throw new NotImplementedException();
    }

    public Task<Pagination<StudentOutput>> GetPgnOfUnRegdStdntAsync(int page, int pageSize, string keyword)
    {

        throw new NotImplementedException();
    }

    public Task<Pagination<StudentOutput>> GetPgnOfRegdStdntAsync(int page, int pageSize, string keyword)
    {

        throw new NotImplementedException();
    }

    public async Task<byte[]> ExportAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "student_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Student> students = await _context.Students.Include(i => i.StudentClass)
            .Where(f => f.IsDeleted == false).OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = students.Select(s => new StudentExport
                {
                    Index = count++,
                    Id = s.Id,
                    Surname = s.Surname,
                    Name = s.Name,
                    ClassName = s.StudentClass.Name,
                    Email = s.Email,
                    Birthday = s.Birthday
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }
}
