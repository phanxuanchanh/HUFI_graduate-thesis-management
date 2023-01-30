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

public class ThesisCommitteeRepository : AsyncSubRepository<ThesisCommittee, ThesisCommitteeInput, ThesisCommitteeOutput, string>, IThesisCommitteeRepository
{
    private HufiGraduateThesisContext _context;

    internal ThesisCommitteeRepository(HufiGraduateThesisContext context)
        : base(context, context.ThesisCommittees)
    {
        _context = context;
    }

    public async Task<DataResponse> AddMemberAsync(CommitteeMemberInput input)
    {
        bool checkExists = await _context.CommitteeMembers
         .AnyAsync(t => t.MemberId == input.MemberId && t.ThesisCommitteeId == input.ThesisCommitteeId);

        if (checkExists)
            return new DataResponse { Status = DataResponseStatus.AlreadyExists, Message = "Thành viên đã được thêm vào tiểu ban!" };

        CommitteeMember committeeMember = new CommitteeMember
        { 
            Id = UidHelper.GetShortUid(),
            ThesisCommitteeId = input.ThesisCommitteeId,
            MemberId = input.MemberId,
            Titles= input.Titles

        };

        await _context.CommitteeMembers.AddAsync(committeeMember);
        await _context.SaveChangesAsync();

        return new DataResponse { Status = DataResponseStatus.Success, Message = "Thêm giảng viên vào tiểu ban thành công!" };

    }

    public async Task<DataResponse> DeleteCommitteeMemberAsync(string thesisCommitteeId, string memberId)
    {
        CommitteeMember committeeMember = await _context.CommitteeMembers
            .Where(t => t.ThesisCommitteeId == thesisCommitteeId && t.MemberId == memberId)
                       .SingleOrDefaultAsync();
        if (committeeMember == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy !"
            };
        _context.CommitteeMembers.Remove(committeeMember);
        _context.SaveChanges();
        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Xóa thành công!"
        };
    }

    public async Task<List<FacultyStaffOutput>> LoadCommitteeMemberAsync(string committeeId)
    {
      return  await _context.CommitteeMembers.Include(t=>t.Member).Where(t => t.ThesisCommitteeId == committeeId && t.IsDeleted == false)
            .Select(s => new FacultyStaffOutput
            {
                Id = s.Member.Id,
                Name = s.Member.Name,
                Surname = s.Member.Surname,

                Email = s.Member.Email

            }).ToListAsync();
       

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
                Semester = (int)s.Council.Semester,
                Year = s.Council.Year
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
