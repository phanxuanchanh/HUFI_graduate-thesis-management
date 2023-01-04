using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Implements;

public class AppRoleRepository : SubRepository<AppRole, AppRoleInput, AppRoleOutput, string>, IAppRoleRepository
{
    private HufiGraduateThesisContext _context;

    public AppRoleRepository(HufiGraduateThesisContext context)
        :base(context, context.AppRoles)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        _genericRepository.IncludeMany(i => i.FacultyStaffs);
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
}
