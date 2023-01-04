using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Implements;

public class AppUserRolesRepository : SubRepository<AppUserRole, AppUserRoleInput, AppUserRoleOutput, string>, IAppUserRolesRepository
{
    private HufiGraduateThesisContext _context;

    public AppUserRolesRepository(HufiGraduateThesisContext context)
        :base(context, context.AppUserRoles)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        _genericRepository.IncludeMany(i => i.Role, i=>i.User) ;
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new AppUserRoleOutput
        {
            UserId = s.UserId,
            RoleId = s.RoleId,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new AppUserRoleOutput
        {
            UserId = s.UserId,
            RoleId = s.RoleId,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }
}
