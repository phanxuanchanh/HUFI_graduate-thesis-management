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
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class FacultyStaffRepository : SubRepository<FacultyStaff, FacultyStaffInput, FacultyStaffOutput, string>, IFacultyStaffRepository
{
    private HufiGraduateThesisContext _context;
    private IHostingEnvironment _hostingEnvironment;
    private IEmailService _emailService;
    private IFileManager _fileManager;

    internal FacultyStaffRepository(HufiGraduateThesisContext context, IHostingEnvironment hostingEnvironment, IEmailService emailService, IFileManager fileManager)
        : base(context, context.FacultyStaffs)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _emailService = emailService;
        _fileManager = fileManager;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.Faculty);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new FacultyStaffOutput
        {
            Id = s.Id,
            Surname = s.Surname,
            Name = s.Name,
            Email = s.Email,
            Phone = s.Phone,
            Faculty = new FacultyOutput
            {
                Id = s.Faculty.Id,
                Name = s.Faculty.Name
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new FacultyStaffOutput
        {
            Id = s.Id,
            Surname = s.Surname,
            Name = s.Name,
            Email = s.Email,
            Phone = s.Phone,
            Address = s.Address,
            Avatar = s.Avatar,
            Birthday = s.Birthday,
            Gender = s.Gender,
            Faculty = new FacultyOutput { 
                Id = s.Faculty.Id,
                Name = s.Faculty.Name
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };

        AdvancedImportSelector = s => new FacultyStaff
        {
            Id = s[1] as string,
            Surname = s[2] as string,
            Name = s[3] as string,
            Email = s[0] as string,
            CreatedAt = DateTime.Now
        };
    }

    public override async Task<DataResponse> ImportAsync(Stream stream, ImportMetadata importMetadata)
    {
        return await _genericRepository.ImportAsync(stream, importMetadata, new ImportSelector<FacultyStaff>
        {
            AdvancedImportSpreadsheet = AdvancedImportSelector
        });
    }

    public override async Task<DataResponse<FacultyStaffOutput>> CreateAsync(FacultyStaffInput input)
    {
        FacultyStaff facultyStaff = new FacultyStaff
        {
            Id = input.Id,
            Surname = input.Surname,
            Name = input.Name,
            Description = input.Description,
            Email = input.Email,
            Phone = input.Phone,
            Address = input.Address,
            Birthday = input.Birthday,
            FacultyId = input.FacultyId,
            Password = "default",
            Salt = "default",
            CreatedAt = DateTime.Now
        };

        await _context.FacultyStaffs.AddAsync(facultyStaff);
        await _context.SaveChangesAsync();

        return new DataResponse<FacultyStaffOutput>
        {
            Status = DataResponseStatus.Success,
            Data = new FacultyStaffOutput
            {
                Id = input.Id,
                Surname = input.Surname,
                Name = input.Name,
                Email = input.Email
            }
        };
    }

    public async Task<ForgotPasswordModel> CreateNewPasswordAsync(NewPasswordModel newPasswordModel)
    {
        FacultyStaff facultyStaff = await _context.FacultyStaffs
           .Where(f => f.Email == newPasswordModel.Email && f.IsDeleted == false)
           .SingleOrDefaultAsync();

        if (facultyStaff == null)
            return new ForgotPasswordModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        facultyStaff.Salt = HashFunctions.GetMD5($"{facultyStaff.Id}|{facultyStaff.Surname}|{facultyStaff.Name}|{DateTime.Now}");
        facultyStaff.Password = BCrypt.Net.BCrypt.HashPassword($"{newPasswordModel.Password}>>>{facultyStaff.Salt}");

        await _context.SaveChangesAsync();

        return new ForgotPasswordModel
        {
            Status = AccountStatus.Success,
            Message = "Đã đổi mật khẩu thành công, hãy thực hiện đăng nhập lại để truy cập vào hệ thống!"
        };
    }

    public async Task<AccountVerificationModel> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
    {
        FacultyStaff facultyStaff = await _context.FacultyStaffs
           .Where(f => f.Email == forgotPasswordModel.Email && f.IsDeleted == false)
           .SingleOrDefaultAsync();

        if (facultyStaff == null)
            return new AccountVerificationModel {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        Random random = new Random();
        facultyStaff.VerificationCode = random.NextString(24);
        facultyStaff.CodeExpTime = DateTime.Now.AddMinutes(5);
        await _context.SaveChangesAsync();

        string mailContent = Resources.EmailResource.account_verification;
        mailContent = mailContent.Replace("@verificationCode", facultyStaff.VerificationCode);

        _emailService.Send(
            facultyStaff.Email,
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
        FacultyStaff facultyStaff = await _context.FacultyStaffs.FindAsync(signInModel.Code);
        if (facultyStaff == null)
            return new SignInResultModel {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        string passwordAndSalt = $"{signInModel.Password}>>>{facultyStaff.Salt}";

        if (!BCrypt.Net.BCrypt.Verify(passwordAndSalt, facultyStaff.Password))
            return new SignInResultModel { 
                Status = AccountStatus.WrongPassword,
                Message = "Mật khẩu không trùng khớp!"
            };

        return new SignInResultModel {
            Status = AccountStatus.Success,
            Message = "Đã đăng nhập vào hệ thống thành công!"
        };
    }

    public async Task<NewPasswordModel> VerifyAccountAsync(AccountVerificationModel accountVerificationModel)
    {
        FacultyStaff facultyStaff = await _context.FacultyStaffs
            .Where(f => f.Email == accountVerificationModel.Email && f.IsDeleted == false)
            .SingleOrDefaultAsync();

        if (facultyStaff == null)
            return new NewPasswordModel { 
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };
        
        if (accountVerificationModel.VerificationCode != facultyStaff.VerificationCode)
            return new NewPasswordModel { 
                Status = AccountStatus.Failed,
                Message = "Mã xác thực không trùng khớp!"
            };

        DateTime currentDatetime = DateTime.Now;
        if (facultyStaff.CodeExpTime < currentDatetime)
            return new NewPasswordModel {
                Status = AccountStatus.Failed,
                Message = "Mã xác nhận đã hết hạn!"
            };

        return new NewPasswordModel { 
            Status = AccountStatus.Success,
            Message = "Thực hiện bước 2 của quá trình lấy lại mật khẩu thành công!",
        };
    }

    public async Task<Pagination<FacultyStaffOutput>> GetPgnHasRoleIdAsync(string roleId, int page, int pageSize, string keyword)
    {
        int n = (page - 1) * pageSize;
        int totalItemCount = await _context.AppUserRoles
            .Where(f => f.RoleId == roleId && f.User.IsDeleted == false)
            .Where(f => f.User.Id.Contains(keyword) || f.User.Surname.Contains(keyword) || f.User.Name.Contains(keyword) || f.User.Email.Contains(keyword))
            .CountAsync();

        List<FacultyStaffOutput> onePageOfData = await _context.AppUserRoles.Include(i => i.User)
            .Where(f => f.RoleId == roleId && f.User.IsDeleted == false)
            .Where(f => f.User.Id.Contains(keyword) || f.User.Surname.Contains(keyword) || f.User.Name.Contains(keyword) || f.User.Email.Contains(keyword))
            .Skip(n).Take(pageSize)
            .Select(s => new FacultyStaffOutput
            {
                Id = s.User.Id,
                Surname = s.User.Surname,
                Name = s.User.Name,
                Email = s.User.Email
            }).ToListAsync();

        return new Pagination<FacultyStaffOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<DataResponse> UpdateProfileAsync(FacultyStaffInput input, FileUploadModel avtUploadModel)
    {
        FacultyStaff facultyStaff_fromDb = await _context.FacultyStaffs.FindAsync(input.Id);
        if (facultyStaff_fromDb == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        facultyStaff_fromDb.Email = input.Email;
        facultyStaff_fromDb.Phone = input.Phone;
        facultyStaff_fromDb.Address = input.Address;
        facultyStaff_fromDb.Birthday = input.Birthday;
        facultyStaff_fromDb.Gender = input.Gender;

        if (avtUploadModel != null)
        {
            avtUploadModel.FileName = $"faculty-staff-avatar_{facultyStaff_fromDb.Id}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{_fileManager.GetExtension(avtUploadModel.ContentType)}";

            _fileManager.SetPath(Path.Combine(_hostingEnvironment.WebRootPath, "avatar", "faculty-staff"));
            _fileManager.Save(avtUploadModel);

            facultyStaff_fromDb.Avatar = $"faculty-staff/{avtUploadModel.FileName}";
        }

        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Cập nhật thông tin cá nhân thành công!"
        };
    }

    public async Task<DataResponse> SetDefaultAvatarAsync(string facultyStaffId)
    {
        FacultyStaff facultyStaff_fromDb = await _context.FacultyStaffs.FindAsync(facultyStaffId);
        if (facultyStaff_fromDb == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        if (facultyStaff_fromDb.Gender == "Nam")
            facultyStaff_fromDb.Avatar = "default-male-profile.png";
        else
            facultyStaff_fromDb.Avatar = "default-female-profile.png";

        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã đặt ảnh đại diện mặc định thành công!"
        };
    }

    public async Task<byte[]> ExportAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "faculty-staff_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<FacultyStaff> facultyStaffs = await _context.FacultyStaffs
            .Where(f => f.IsDeleted == false).OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = facultyStaffs.Select(s => new FacultyStaffExport
                {
                    Index = count++,
                    Surname = s.Surname,
                    Name = s.Name,
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
