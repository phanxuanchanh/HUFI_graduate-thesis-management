using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Implements;

public class TopicRepository : SubRepository<Topic, TopicInput, TopicOutput, string>, ITopicRepository
{
    private HufiGraduateThesisContext _context;

    internal TopicRepository(HufiGraduateThesisContext context) 
        : base(context, context.Topics)
    {
        _context = context;
        GenerateUidOptions = UidOptions.ShortUid;
    }

    protected override void ConfigureIncludes()
    {
        
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new TopicOutput
        {
           Id= s.Id,
           Name= s.Name,
           Description= s.Description,
           CreatedAt = s.CreatedAt,
           UpdatedAt = s.UpdatedAt,
           DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new TopicOutput
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
