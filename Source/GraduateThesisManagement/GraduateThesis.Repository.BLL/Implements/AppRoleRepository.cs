using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class AppRoleRepository : AsyncSubRepository<AppRole, AppRoleInput, AppRoleOutput, string>, IAppRoleRepository
{
    private HufiGraduateThesisContext _context;

    public AppRoleRepository(HufiGraduateThesisContext context)
        :base(context, context.AppRoles)
    {
        _context = context;
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

    protected override void SetOutputMapper(AppRole entity, AppRoleOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }

    protected override void SetMapperToUpdate(AppRoleInput input, AppRole entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetMapperToCreate(AppRoleInput input, AppRole entity)
    {
        entity.Id = UidHelper.GetMicrosoftUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CreatedAt = DateTime.Now;
    }

    public async Task<DataResponse> GrantToPageAsync(AppRoleMappingInput input)
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

    public async Task<DataResponse> RevokeFromPageAsync(AppRoleMappingInput input)
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
