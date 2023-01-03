using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;

namespace GraduateThesis.Repository.BLL.Implements;

public class TopicRepository : SubRepository<Topic, TopicInput, TopicOutput, string>, ITopicRepository
{
    internal TopicRepository(HufiGraduateThesisContext context) 
        : base(context, context.Topics)
    {
        
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

        SimpleImportSelector = r => new Topic
        {
            Id = UidHelper.GetShortUid(),
            Name = r[1] as string,
            Description = r[2] as string,
            CreatedAt = DateTime.Now
        };
    }
}
