using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Implements;

public class TrainingLevelRepository : SubRepository<TrainingLevel, TrainingLevelInput, TrainingLevelOutput, string>, ITrainingLevelRepository
{
    private HufiGraduateThesisContext _context;

    internal TrainingLevelRepository(HufiGraduateThesisContext context)
        : base(context, context.TrainingLevels)
    {
        _context = context;
        GenerateUidOptions = UidOptions.ShortUid;
    }

    protected override void ConfigureIncludes()
    {
        _genericRepository.IncludeMany(i => i.Theses);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new TrainingLevelOutput
        {
            Id = s.Id,
            Name = s.Name,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new TrainingLevelOutput
        {
            Id = s.Id,
            Name = s.Name,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }
}
