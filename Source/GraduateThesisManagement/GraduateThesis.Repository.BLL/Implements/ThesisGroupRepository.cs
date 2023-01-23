using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class ThesisGroupRepository : SubRepository<ThesisGroup, ThesisGroupInput, ThesisGroupOutput, string>, IThesisGroupRepository
{
    private HufiGraduateThesisContext _context;

    public ThesisGroupRepository(HufiGraduateThesisContext context)
        : base(context, context.ThesisGroups)
    {
        _context = context;
        GenerateUidOptions = UidOptions.ShortUid;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.Theses);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new ThesisGroupOutput
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
        SingleSelector = s => new ThesisGroupOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            StudentQuantity = s.StudentQuantity,
            Notes = s.Notes,
            Thesis = s.Theses.Select(ts => new ThesisOutput
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
    }

    public override async Task<ThesisGroupOutput> GetAsync(string id)
    {
        ThesisGroupOutput thesisGroup = await base.GetAsync(id);

        thesisGroup.Students = await _context.ThesisGroupDetails.Include(i => i.Student)
            .Where(s => s.StudentThesisGroupId == id && s.IsDeleted == false)
            .Select(s => new StudentOutput { Id = s.Student.Id, Name = s.Student.Name })
            .ToListAsync();

        return thesisGroup;
    }

    public async Task<ThesisGroupOutput> GetAsync(string studentId, string thesisId)
    {
        ThesisGroupOutput thesisGroup = await _context.ThesisGroupDetails
            .Where(gd => gd.StudentId == studentId && gd.IsDeleted == false)
            .Join(
                _context.ThesisGroups.Where(t => t.IsDeleted == false),
                groupDetail => groupDetail.StudentThesisGroupId,
                group => group.Id,
                (groupDetail, group) => group
            ).Join(
                _context.Theses.Where(t => t.Id == thesisId && t.IsDeleted == false),
                combined => combined.Id,
                thesis => thesis.Id,
                (combined, thesis) => new ThesisGroupOutput
                {
                    Id = combined.Id,
                    Name = combined.Name,
                    Description = combined.Description
                }
            ).SingleOrDefaultAsync();

        thesisGroup.Students = await _context.ThesisGroupDetails.Include(i => i.Student)
            .Where(s => s.StudentThesisGroupId == thesisGroup.Id && s.IsDeleted == false)
            .Select(s => new StudentOutput { Id = s.Student.Id, Name = s.Student.Name })
            .ToListAsync();

        return thesisGroup;
    }

    public async Task<List<ThesisGroupOutput>> GetListAsync(string studentId)
    {
        return await _context.ThesisGroupDetails.Include(i => i.StudentThesisGroup)
            .Where(gd => gd.StudentId == studentId && gd.IsDeleted == false)
            .Select(s => new ThesisGroupOutput
            {
                Id = s.StudentThesisGroup.Id,
                Name = s.StudentThesisGroup.Name,
                Description = s.StudentThesisGroup.Description
            }).ToListAsync();
    }

    public async Task<DataResponse> JoinToGroupAsync(string studentId, string thesisGroupId)
    {
        ThesisGroupDetail thesisGroupDetail = await _context.ThesisGroupDetails
            .Where(gd => gd.StudentId == studentId && gd.StudentThesisGroupId == thesisGroupId && gd.IsDeleted == false)
            .SingleOrDefaultAsync();

        if (thesisGroupDetail == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy nhóm có mã này!"
            };

        thesisGroupDetail.IsApproved = true;
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Bạn đã vào nhóm thành công!"
        };
    }

    public async Task<DataResponse> DenyFromGroupAsync(string studentId, string thesisGroupId)
    {
        ThesisGroupDetail thesisGroupDetail = await _context.ThesisGroupDetails
            .Where(gd => gd.StudentId == studentId && gd.StudentThesisGroupId == thesisGroupId && gd.IsDeleted == false)
            .SingleOrDefaultAsync();

        if (thesisGroupDetail == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy nhóm có mã này!"
            };

        thesisGroupDetail.IsApproved = false;
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Bạn đã từ chối tham gia nhóm thành công!"
        };
    }
}
