using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class ThesisGroupRepository : AsyncSubRepository<ThesisGroup, ThesisGroupInput, ThesisGroupOutput, string>, IThesisGroupRepository
{
    private HufiGraduateThesisContext _context;

    public ThesisGroupRepository(HufiGraduateThesisContext context)
        : base(context, context.ThesisGroups)
    {
        _context = context;
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

    protected override void SetOutputMapper(ThesisGroup entity, ThesisGroupOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }

    protected override void SetMapperToUpdate(ThesisGroupInput input, ThesisGroup entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetMapperToCreate(ThesisGroupInput input, ThesisGroup entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.CreatedAt = DateTime.Now;
    }

    public override async Task<ThesisGroupOutput> GetAsync(string id)
    {
        ThesisGroupOutput thesisGroup = await base.GetAsync(id);

        //thesisGroup.Students = await _context.ThesisGroupDetails.Include(i => i.Student)
        //    .Where(s => s.StudentThesisGroupId == id && s.IsDeleted == false)
        //    .Select(s => new StudentOutput { Id = s.Student.Id, Name = s.Student.Name })
        //    .ToListAsync();

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

        //thesisGroup.Students = await _context.ThesisGroupDetails.Include(i => i.Student)
        //    .Where(s => s.StudentThesisGroupId == thesisGroup.Id && s.IsDeleted == false)
        //    .Select(s => new StudentOutput { Id = s.Student.Id, Name = s.Student.Name })
        //    .ToListAsync();

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

        int studentQuantity = await _context.ThesisGroupDetails
            .Where(gd => gd.StudentThesisGroupId == thesisGroupId && gd.IsApproved == true && gd.IsDeleted == false)
            .CountAsync();

        ThesisGroup thesisGroup = await _context.ThesisGroups
            .Where(tg => tg.Id == thesisGroupId && tg.IsDeleted == false)
            .SingleOrDefaultAsync();

        thesisGroup.StudentQuantity = studentQuantity;
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

        int studentQuantity = await _context.ThesisGroupDetails
            .Where(gd => gd.StudentThesisGroupId == thesisGroupId && gd.IsApproved == true && gd.IsDeleted == false)
            .CountAsync();

        ThesisGroup thesisGroup = await _context.ThesisGroups
            .Where(tg => tg.Id == thesisGroupId && tg.IsDeleted == false)
            .SingleOrDefaultAsync();

        thesisGroup.StudentQuantity = studentQuantity;
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Bạn đã từ chối tham gia nhóm thành công!"
        };
    }

    public async Task<List<ThesisGroupDtOutput>> GetGrpsByStdntIdAsync(string studentId)
    {
        return await _context.ThesisGroupDetails.Include(i => i.StudentThesisGroup)
            .Where(gd => gd.StudentId == studentId && gd.IsDeleted == false && gd.StudentThesisGroup.IsDeleted == false)
            .Join(
                _context.Theses.Where(t => t.IsDeleted == false),
                groupDetail => groupDetail.StudentThesisGroup.Id,
                thesis => thesis.ThesisGroupId,
                (groupDetail, thesis) => new ThesisGroupDtOutput
                {
                    ThesisId = thesis.Id,
                    ThesisName = thesis.Name,
                    GroupId = groupDetail.StudentThesisGroup.Id,
                    GroupName = groupDetail.StudentThesisGroup.Name,
                    GroupDescription = groupDetail.StudentThesisGroup.Description,
                    StudentQuantity = groupDetail.StudentThesisGroup.StudentQuantity,
                    RegistrationDate = groupDetail.StudentThesisGroup.CreatedAt,
                    GroupNotes = groupDetail.StudentThesisGroup.Notes,
                    Group_IsCompleted = groupDetail.StudentThesisGroup.IsCompleted,
                    Group_IsFinished = groupDetail.StudentThesisGroup.IsFinished,
                    MemberNotes = groupDetail.Notes,
                    Member_IsApproved = groupDetail.IsApproved,
                    Member_IsCompleted = groupDetail.IsCompleted,
                    Member_IsFinished = groupDetail.IsFinished
                }
            ).ToListAsync();
    }
}
