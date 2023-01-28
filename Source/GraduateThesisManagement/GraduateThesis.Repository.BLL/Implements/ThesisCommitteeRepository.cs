using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;

namespace GraduateThesis.Repository.BLL.Implements;

public class ThesisCommitteeRepository : AsyncSubRepository<ThesisCommittee, ThesisCommitteeInput, ThesisCommitteeOutput, string>, IThesisCommitteeRepository
{
    private HufiGraduateThesisContext _context;

    internal ThesisCommitteeRepository(HufiGraduateThesisContext context)
        :base(context, context.ThesisCommittees)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new ThesisCommitteeOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Council = new CouncilOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Semester= (int)s.Council.Semester,
                Year=s.Council.Year
            }
        };

        ListSelector = ListSelector;
        SingleSelector = s => new ThesisCommitteeOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Council = new CouncilOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Semester = (int)s.Council.Semester,
                Year = s.Council.Year
            }
        };
    }

    protected override void SetMapperToCreate(ThesisCommitteeInput input, ThesisCommittee entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CouncilId = input.CouncilId;
        entity.CreatedAt = DateTime.Now;

    }

    protected override void SetMapperToUpdate(ThesisCommitteeInput input, ThesisCommittee entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CouncilId = input.CouncilId;
        entity.UpdatedAt = DateTime.Now;

    }

    protected override void SetOutputMapper(ThesisCommittee entity, ThesisCommitteeOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;

    }
}
