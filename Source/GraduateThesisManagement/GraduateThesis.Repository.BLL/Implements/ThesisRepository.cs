using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class ThesisRepository : SubRepository<Thesis, ThesisInput, ThesisOutput, string>, IThesisRepository
{
    private HufiGraduateThesisContext _context;

    internal ThesisRepository(HufiGraduateThesisContext context)
        : base(context, context.Theses)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(
            i => i.Topic, 
            i => i.ThesisGroup, 
            i => i.TrainingForm, 
            i => i.TrainingLevel, 
            i => i.Specialization
        );
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new ThesisOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            MaxStudentNumber = s.MaxStudentNumber,
            ThesisGroupId = s.ThesisGroupId
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new ThesisOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            MaxStudentNumber = s.MaxStudentNumber,
            SourceCode = s.SourceCode,
            Notes = s.Notes,
            TopicId = s.TopicId,
            TopicClass = new TopicOutput
            {
                Name = s.Topic.Name,
                Description = s.Topic.Description,
                Id = s.Topic.Id,
            },
            StudentThesisGroup = (s.ThesisGroup == null) ? null : new StudentThesisGroupOutput
            {
                Id = s.ThesisGroup.Id,
                Name = s.ThesisGroup!.Name,
                Description = s.ThesisGroup!.Description,
                StudentQuantity = s.ThesisGroup!.StudentQuantity
            },
            TrainingForm = (s.TrainingForm == null) ? null : new TrainingFormOutput
            {
                Id = s.TrainingForm.Id,
                Name = s.TrainingForm!.Name,
            },
            TrainingLevel = new TrainingLevelOutput
            {
                Id = s.TrainingLevel.Id,
                Name = s.TrainingLevel!.Name,
            },
            Specialization = (s.Specialization == null) ? null : new SpecializationOutput
            {
                Id = s.Specialization.Id,
                Name = s.Specialization!.Name,
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }

    public async Task<DataResponse> DoThesisRegisterAsync(ThesisRegisterInput thesisRegisterInput)
    {
        IDbContextTransaction dbContextTransaction = null;
        try
        {
            dbContextTransaction = _context.Database.BeginTransaction();

            Thesis thesis = await _context.Theses.FindAsync(thesisRegisterInput.ThesisId);
            if (thesis == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };

            string groupId = UidHelper.GetShortUid();

            StudentThesisGroup studentThesisGroup = new StudentThesisGroup
            {
                Id = groupId,
                Name = thesisRegisterInput.GroupName,
                Description = thesisRegisterInput.GroupDescription,
                StudentQuantity = 0
            };

            await _context.StudentThesisGroups.AddAsync(studentThesisGroup);
            await _context.SaveChangesAsync();

            thesis.ThesisGroupId = groupId;
            await _context.SaveChangesAsync();

            List<StudentThesisGroupDetail> thesisGroupDetails = new List<StudentThesisGroupDetail>();
            string[] studentIdList = thesisRegisterInput.StudentIdList.Split(';');

            foreach (string studentId in studentIdList)
            {
                thesisGroupDetails.Add(new StudentThesisGroupDetail
                {
                    StudentThesisGroupId = groupId,
                    StudentId = studentId,
                    IsApproved = false
                });
            }

            await _context.StudentThesisGroupDetails.AddRangeAsync(thesisGroupDetails);
            await _context.SaveChangesAsync();

            await dbContextTransaction.CommitAsync();

            return new DataResponse { Status = DataResponseStatus.Success };
        }
        catch (Exception ex)
        {
            if (dbContextTransaction != null)
                await dbContextTransaction.RollbackAsync();

            throw new Exception("The process was aborted because of an error!", ex);
        }
    }

    public async Task<DataResponse> SubmitThesisAsync(string thesisId, string thesisGroupId)
    {
        Thesis thesis = await _context.Theses.FindAsync(thesisId);
        if (thesis == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy đề tài có mã này!"
            };

        if (thesis.ThesisGroupId == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Đề tài này chưa được đăng ký ! Bạn không được phép nộp đề tài này"
            };

        if (thesis.ThesisGroupId != thesisGroupId)
            return new DataResponse
            {
                Status = DataResponseStatus.InvalidData,
                Message = "Đề tài này không thuộc nhóm của bạn!"
            };

        thesis.Finished = true;
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Bạn đã nộp đề tài thành công!"
        };
    }
}
