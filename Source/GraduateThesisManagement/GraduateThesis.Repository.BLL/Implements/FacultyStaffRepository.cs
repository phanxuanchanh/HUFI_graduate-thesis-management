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
using System;
using System.Collections.Generic;
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

    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new FacultyStaffOutput
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            Phone = s.Phone
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new FacultyStaffOutput
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            Phone = s.Phone,
            Address = s.Address,
            Avatar = s.Avatar,
            Birthday = s.Birthday,
            Gender = s.Gender,
            Faculty = new FacultyOutput { 
                Id = s.Faculty.Id,
                Name = s.Faculty.Name
            }
        };

        SimpleImportSelector = r => new FacultyStaff
        {
            Id = r[0] as string,
            FullName = r[1] as string,
            Email = r[2] as string,
            Phone = r[3] as string,
            Description = r[4] as string,
            CreatedAt = DateTime.Now
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

        facultyStaff.Salt = HashFunctions.GetMD5($"{facultyStaff.Id}|{facultyStaff.FullName}|{DateTime.Now}");
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

        _emailService.Send(
            facultyStaff.Email,
            "Khôi phục mật khẩu",
            $"Mã xác nhận của bạn là: {facultyStaff.VerificationCode}"
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
            .Where(f => f.RoleId == roleId && f.User.IsDeleted == false).CountAsync();

        List<FacultyStaffOutput> onePageOfData = await _context.AppUserRoles.Include(i => i.User)
            .Where(f => f.RoleId == roleId && f.User.IsDeleted == false)
            .Where(f => f.User.Id.Contains(keyword) || f.User.FullName.Contains(keyword) || f.User.Email.Contains(keyword))
            .Skip(n).Take(pageSize)
            .Select(s => new FacultyStaffOutput
            {
                Id = s.User.Id,
                FullName = s.User.FullName,
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
}
