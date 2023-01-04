using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;

namespace GraduateThesis.Repository.BLL.Implements;

public class StudentClassRepository : SubRepository<StudentClass, StudentClassInput, StudentClassOutput, string>, IStudentClassRepository
{
    private HufiGraduateThesisContext _context;

    internal StudentClassRepository(HufiGraduateThesisContext context)
        :base(context, context.StudentClasses)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        _genericRepository.IncludeMany(i => i.Faculty);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new StudentClassOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            StudentQuantity= s.StudentQuantity,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new StudentClassOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            StudentQuantity = s.StudentQuantity,
            Faculty = new FacultyOutput
            {
                Id = s.Faculty.Id,
                Name = s.Faculty.Name,
                Description = s.Faculty.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        SimpleImportSelector = r => new StudentClass
        {
            Id = r[0] as string,
            Name = r[1] as string,
            Description = r[2] as string,
            CreatedAt = DateTime.Now
        };
    }
}
