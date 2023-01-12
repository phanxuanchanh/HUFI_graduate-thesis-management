using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class AppPageRepository : SubRepository<AppPage, AppPageInput, AppPageOutput, string>, IAppPageRepository
{
    private HufiGraduateThesisContext _context;

    public AppPageRepository(HufiGraduateThesisContext context) 
        : base(context, context.AppPages)
    {
        _context = context;
        GenerateUidOptions = UidOptions.ShortUid;
    }

    protected override void ConfigureIncludes()
    {
        
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new AppPageOutput
        {
            Id = s.Id,
            PageName = s.PageName,
            ControllerName = s.ControllerName,
            ActionName = s.ActionName
        };

        ListSelector = PaginationSelector;

        SingleSelector = s => new AppPageOutput
        {
            Id = s.Id,
            PageName = s.PageName,
            ControllerName = s.ControllerName,
            ActionName = s.ActionName,
            Area = s.Area,
            Path = s.Path,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };
    }

    public async Task<DataResponse> GrantAsync(AppRoleMappingInput input)
    {
        bool checkRolesExists = await _context.AppRoles
            .AnyAsync(r => r.Id == input.RoleId && r.IsDeleted == false);
        if (!checkRolesExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy quyền có Id này!"
            };

        bool checkPageExists = await _context.AppPages
            .AnyAsync(p => p.Id == input.PageId && p.IsDeleted == false);
        if (!checkPageExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy trang có Id này!"
            };

        bool checkExists = await _context.AppRoleMappings
            .AnyAsync(rm => rm.RoleId == input.RoleId && rm.PageId == input.PageId);

        if (checkExists)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Đã gán quyền với trang này, không thể thực hiện gán trùng!"
            };

        AppRoleMapping appRoleMapping = new AppRoleMapping
        {
            RoleId = input.RoleId,
            PageId = input.PageId,
            CreatedAt = DateTime.Now
        };

        await _context.AppRoleMappings.AddAsync(appRoleMapping);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã gán quyền cho trang thành công!"
        };
    }

    public async Task<DataResponse> RevokeAsync(AppRoleMappingInput input)
    {
        bool checkRolesExists = await _context.AppRoles
            .AnyAsync(r => r.Id == input.RoleId && r.IsDeleted == false);
        if (!checkRolesExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy quyền có Id này!"
            };

        bool checkPageExists = await _context.AppPages
            .AnyAsync(p => p.Id == input.PageId && p.IsDeleted == false);
        if (!checkPageExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy trang có Id này!"
            };

        AppRoleMapping appRoleMapping = await _context.AppRoleMappings
            .FindAsync(input.RoleId, input.PageId);

        if (appRoleMapping == null)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Không thể thu hồi quyền vì không tồn tại trong hệ thống!"
            };

        _context.AppRoleMappings.Remove(appRoleMapping);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã thu hồi quyền được gán cho trang thành công!"
        };
    }
}
