using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Dml;
using System;
using System.Security.Policy;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class AppRoleRepository : SubRepository<AppRole, AppRoleInput, AppRoleOutput, string>, IAppRoleRepository
{
    private HufiGraduateThesisContext _context;

    public AppRoleRepository(HufiGraduateThesisContext context)
        :base(context, context.AppRoles)
    {
        _context = context;
        GenerateUidOptions = UidOptions.MicrosoftUid;
    }

    protected override void ConfigureIncludes()
    {

    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new AppRoleOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new AppRoleOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }

    public async Task<DataResponse> GrantAsync(AppUserRoleInput input)
    {
        bool checkRolesExists = await _context.AppRoles
            .AnyAsync(r => r.Id == input.RoleId && r.IsDeleted == false);
        if (!checkRolesExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy quyền có Id này!"
            };

        bool checkUserExists = await _context.FacultyStaffs
            .AnyAsync(f => f.Id == input.UserId && f.IsDeleted == false);
        if (!checkUserExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy người dùng có Id này!"
            };

        bool checkExists = await _context.AppUserRoles
            .AnyAsync(ur => ur.RoleId == input.RoleId && ur.UserId == input.UserId);

        if (checkExists)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Quyền đã được gán vào cho người dùng, không thể thực hiện gán trùng!"
            };

        AppUserRole appUserRole = new AppUserRole
        {
            RoleId = input.RoleId,
            UserId = input.UserId,
            CreatedAt = DateTime.Now
        };

        await _context.AppUserRoles.AddAsync(appUserRole);
        await _context.SaveChangesAsync();
        
        return new DataResponse { 
            Status = DataResponseStatus.Success, 
            Message = "Đã gán quyền thành công!" 
        };
    }

    public async Task<DataResponse> RevokeAsync(AppUserRoleInput input)
    {
        bool checkRolesExists = await _context.AppRoles
            .AnyAsync(r => r.Id == input.RoleId && r.IsDeleted == false);
        if (!checkRolesExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy quyền có Id này!"
            };

        bool checkUserExists = await _context.FacultyStaffs
            .AnyAsync(f => f.Id == input.UserId && f.IsDeleted == false);
        if (!checkUserExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy người dùng có Id này!"
            };

        AppUserRole appUserRole = await _context.AppUserRoles
            .FindAsync(input.UserId, input.RoleId);

        if (appUserRole == null)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Không thể thu hồi quyền vì không tồn tại trong hệ thống!"
            };

        _context.AppUserRoles.Remove(appUserRole);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã thu hồi quyền thành công!"
        };
    }
}
