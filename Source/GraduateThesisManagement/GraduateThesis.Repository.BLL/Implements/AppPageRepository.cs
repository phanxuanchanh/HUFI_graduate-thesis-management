using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public override async Task<Pagination<AppPageOutput>> GetPaginationAsync(Pagination<AppPageOutput> pagination)
    {
        pagination.OrderBy = "ControllerName";
        pagination.OrderOptions = OrderOptions.ASC;

        return await base.GetPaginationAsync(pagination);
    }

    public async Task<Pagination<AppPageOutput>> GetPgnHasRoleIdAsync(string roleId, int page, int pageSize, string keyword)
    {
        int n = (page - 1) * pageSize;
        int totalItemCount = await _context.AppRoleMappings
            .Where(rm => rm.RoleId == roleId && rm.Page.IsDeleted == false).CountAsync();

        List<AppPageOutput> onePageOfData = await _context.AppRoleMappings.Include(i => i.Page)
            .Where(rm => rm.RoleId == roleId && rm.Page.IsDeleted == false)
            .Where(rm => rm.Page.Id.Contains(keyword) || rm.Page.PageName.Contains(keyword) || rm.Page.ControllerName.Contains(keyword) || rm.Page.ActionName.Contains(keyword))
            .Skip(n).Take(pageSize)
            .Select(s => new AppPageOutput
            {
                Id = s.Page.Id,
                PageName = s.Page.PageName,
                ControllerName = s.Page.ControllerName,
                ActionName = s.Page.ActionName
            }).ToListAsync();

        return new Pagination<AppPageOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }
}
