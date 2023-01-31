using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class ThesisCommitteeRepository : AsyncSubRepository<ThesisCommittee, ThesisCommitteeInput, ThesisCommitteeOutput, string>, IThesisCommitteeRepository
{
    private HufiGraduateThesisContext _context;

    internal ThesisCommitteeRepository(HufiGraduateThesisContext context)
        : base(context, context.ThesisCommittees)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.Council);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new ThesisCommitteeOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CouncilId = s.CouncilId,
            Council = new CouncilOutput
            {
                Id = s.Council.Id,
                Name = s.Council.Name
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };

        ListSelector = ListSelector;
        SingleSelector = s => new ThesisCommitteeOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Council = new CouncilOutput
            {
                Id = s.Council.Id,
                Name = s.Council.Name,
                Description = s.Council.Description,
                Semester = s.Council.Semester,
                Year = s.Council.Year
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
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

    public override async Task<ThesisCommitteeOutput> GetAsync(string id)
    {
        ThesisCommitteeOutput thesisCommittee = await base.GetAsync(id);

        if(thesisCommittee != null)
        {
            thesisCommittee.Members = await _context.CommitteeMembers
            .Where(cm => cm.ThesisCommitteeId == thesisCommittee.Id)
            .Select(s => new CommitteeMemberOutput
            {
                MemberId = s.MemberId,
                Surname = s.Member.Surname,
                Name = s.Member.Name,
                Email = s.Member.Email,
                Titles = s.Titles
            }).ToListAsync();
        }

        return thesisCommittee;
    }

    public async Task<DataResponse> AddMemberAsync(CommitteeMemberInput input)
    {
        bool checkExists = await _context.CommitteeMembers
         .AnyAsync(t => t.MemberId == input.MemberId && t.ThesisCommitteeId == input.CommitteeId);

        if (checkExists)
            return new DataResponse { 
                Status = DataResponseStatus.AlreadyExists, 
                Message = "Thành viên đã được thêm vào tiểu ban!" 
            };

        CommitteeMember committeeMember = new CommitteeMember
        {
            Id = UidHelper.GetShortUid(),
            ThesisCommitteeId = input.CommitteeId,
            MemberId = input.MemberId,
            Titles = input.Titles
        };

        await _context.CommitteeMembers.AddAsync(committeeMember);
        await _context.SaveChangesAsync();

        return new DataResponse { 
            Status = DataResponseStatus.Success, 
            Message = "Thêm giảng viên vào tiểu ban thành công!" 
        };
    }

    public async Task<DataResponse> DeleteMemberAsync(string committeeId, string memberId)
    {
        CommitteeMember committeeMember = await _context.CommitteeMembers
            .Where(t => t.ThesisCommitteeId == committeeId && t.MemberId == memberId)
                       .SingleOrDefaultAsync();

        if (committeeMember == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy thành viên này trong tiểu ban!"
            };

        _context.CommitteeMembers.Remove(committeeMember);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Xóa thành công!"
        };
    }
}