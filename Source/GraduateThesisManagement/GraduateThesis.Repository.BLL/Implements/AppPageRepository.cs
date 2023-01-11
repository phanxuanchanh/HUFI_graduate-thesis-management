using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Implements;

public class AppPageRepository : SubRepository<AppPage, AppPageInput, AppPageOutput, string>, IAppPageRepository
{
    public AppPageRepository(HufiGraduateThesisContext context) 
        : base(context, context.AppPages)
    {
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
}
