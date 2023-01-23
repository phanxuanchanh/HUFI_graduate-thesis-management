using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
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

    public async Task<List<ThesisRevisionOutput>> GetRevsByThesisIdAsync(string thesisId)
    {
        return await _context.ThesisRevisions.Where(tr => tr.ThesisId == thesisId && tr.IsDeleted == false)
            .Select(s => new ThesisRevisionOutput
            {
                Id = s.Id,
                Title = s.Title,
                Summary = s.Summary,
                DocumentFile = s.DocumentFile,
                PresentationFile = s.PresentationFile,
                PdfFile = s.PdfFile,
                SourceCode = s.SourceCode,
                Reviewed = s.Reviewed,
                LecturerComment = s.LecturerComment,
                Point = s.Point,
                Thesis = new ThesisOutput
                {
                    Id = s.Thesis.Id,
                    Name = s.Thesis.Name,
                },
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            }).OrderByDescending(tr => tr.CreatedAt).ToListAsync();
    }

    public async Task<DataResponse> ReviewRevision(ThesisRevRevisionOutput thesisRevRevision)
    {
        ThesisRevision thesisRevision = await _context.ThesisRevisions.FindAsync(thesisRevRevision.RevisionId);
        if (thesisRevision == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy phiên bản có mã này!"
            };

        thesisRevision.Reviewed = true;
        thesisRevision.LecturerComment = thesisRevRevision.Comment;
        thesisRevision.Point = thesisRevRevision.Point;

        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.NotFound,
            Message = "Đã đánh giá cho quá trình thực hiện đề tài thành công!"
        };
    }
}
