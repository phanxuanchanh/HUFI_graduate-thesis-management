using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;
using System.Linq;

namespace GraduateThesis.Repository.BLL.Implements;

public class FacultyRepository : SubRepository<Faculty, FacultyInput, FacultyOutput, string>, IFacultyRepository
{
    private HufiGraduateThesisContext _context;

    public FacultyRepository(HufiGraduateThesisContext context)
        : base(context, context.Faculties)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.FacultyStaffs);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new FacultyOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new FacultyOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            FacultyStaffs = s.FacultyStaffs.Select(f => new FacultyStaffOutput
            {
                Id = f.Id,
                FullName = f.FullName
            }).ToList(),
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        SimpleImportSelector = r => new Faculty
        {
            Id = UidHelper.GetShortUid(),
            Name = r[1] as string,
            Description = r[2] as string,
            CreatedAt = DateTime.Now
        };
    }
}
