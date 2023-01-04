using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System.Linq;

namespace GraduateThesis.Repository.BLL.Implements;

public class StudentThesisGroupRepository : SubRepository<StudentThesisGroup, StudentThesisGroupInput, StudentThesisGroupOutput, string>, IStudentThesisGroupRepository
{
    private HufiGraduateThesisContext _context;

    public StudentThesisGroupRepository(HufiGraduateThesisContext context)
        : base(context, context.StudentThesisGroups)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        //_genericRepository.IncludeMany(i => i.);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new StudentThesisGroupOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            StudentQuantity = s.StudentQuantity,
            Notes = s.Notes,
            Thesis = s.Theses.Select(ts=> new ThesisOutput
            {
                Id = ts.Id,
                Name = ts.Name,
                Description = ts.Description,
                SourceCode = ts.SourceCode,
                Notes = ts.Notes,
                TopicId = ts.Notes,
                MaxStudentNumber = ts.MaxStudentNumber,
               
            }).FirstOrDefault()
            

        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new StudentThesisGroupOutput
        {

        };
    }
}
