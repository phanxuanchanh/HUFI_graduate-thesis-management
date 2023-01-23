using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class ThesisRevisionRepository : SubRepository<ThesisRevision, ThesisRevisionInput, ThesisRevisionOutput, string>, IThesisRevisionRepository
{
    private HufiGraduateThesisContext _context;

    internal ThesisRevisionRepository(HufiGraduateThesisContext context)
        : base(context, context.ThesisRevisions)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.Thesis);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new ThesisRevisionOutput
        {
            Id = s.Id,
            Title = s.Title,
            Summary = s.Summary
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new ThesisRevisionOutput
        {
            Id = s.Id,
            Title = s.Title,
            Summary = s.Summary,
            DocumentFile = s.DocumentFile,
            PresentationFile = s.PresentationFile,
            PdfFile = s.PdfFile,
            SourceCode = s.SourceCode,
            Thesis = new ThesisOutput
            {
                Id = s.Thesis.Id,
                Name = s.Thesis.Name,
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }

    public async Task<List<ThesisRevisionOutput>> GetRevByThesisIdAsync(string thesisId)
    {
        return await _context.ThesisRevisions.Where(tv => tv.ThesisId == thesisId && tv.IsDeleted == false)
            .Select(s => new ThesisRevisionOutput
            {
                Id = s.Id,
                Title = s.Title,
                Summary = s.Summary,
                DocumentFile = s.DocumentFile,
                PresentationFile = s.PresentationFile,
                PdfFile = s.PdfFile,
                SourceCode = s.SourceCode,
                Thesis = new ThesisOutput
                {
                    Id = s.Thesis.Id,
                    Name = s.Thesis.Name,
                },
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            }).ToListAsync();
    }
}
