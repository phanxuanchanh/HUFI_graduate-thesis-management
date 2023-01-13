using GraduateThesis.ApplicationCore.Context;
using Microsoft.EntityFrameworkCore;

namespace GraduateThesis.ApplicationCore.Authorization;

public class RoleManager : IRoleManager
{
    private readonly AppDbContext _context;

    public RoleManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsValidAsync(string userId, string controllerName, string actionName)
    {
        int count = await _context.AppUserRoles.Where(ur => ur.UserId == userId)
            .Join(
                _context.AppRoles.Where(r => r.IsDeleted == false),
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => new { RoleId = role.Id }
            ).Join(
                _context.AppRoleMappings,
                combined1 => combined1.RoleId,
                roleMapping => roleMapping.RoleId,
                (combined1, roleMapping) => new { RoleId = combined1.RoleId, PageId = roleMapping.PageId }
            ).Join(
                _context.AppPages.Where(p => p.ControllerName == controllerName && p.ActionName == actionName && p.IsDeleted == false),
                combined2 => combined2.PageId,
                page => page.Id,
                (combined2, page) => new { page }
            ).CountAsync();

        return (count > 0);
    }
}
