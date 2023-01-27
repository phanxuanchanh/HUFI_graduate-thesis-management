using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using X.PagedList;

namespace GraduateThesis.Repository.BLL.Implements;

public class ThesisRepository : AsyncSubRepository<Thesis, ThesisInput, ThesisOutput, string>, IThesisRepository
{
    private HufiGraduateThesisContext _context;
    private IEmailService _emailService;

    internal ThesisRepository(HufiGraduateThesisContext context, IEmailService emailService)
        : base(context, context.Theses)
    {
        _context = context;
        _emailService = emailService;
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
            MaxStudentNumber = s.MaxStudentNumber,
            ThesisGroupId = s.ThesisGroupId,
            Lecturer = new FacultyStaffOutput
            {
                Id = s.Lecture.Id,
                Surname = s.Lecture.Surname,
                Name = s.Lecture.Name
            }
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new ThesisOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            MaxStudentNumber = s.MaxStudentNumber,
            Credits = s.Credits,
            SourceCode = s.SourceCode,
            Notes = s.Notes,
            TopicId = s.TopicId,
            Topic = new TopicOutput
            {
                Id = s.Topic.Id,
                Name = s.Topic.Name,
                Description = s.Topic.Description
            },
            LectureId = s.LectureId,
            Lecturer = new FacultyStaffOutput
            {
                Id = s.Lecture.Id,
                Surname = s.Lecture.Surname,
                Name = s.Lecture.Name
            },
            ThesisGroupId = s.ThesisGroupId,
            ThesisGroup = (s.ThesisGroup == null) ? null : new ThesisGroupOutput
            {
                Id = s.ThesisGroup.Id,
                Name = s.ThesisGroup!.Name,
                Description = s.ThesisGroup!.Description,
                StudentQuantity = s.ThesisGroup!.StudentQuantity
            },
            TrainingFormId = s.TrainingFormId,
            TrainingForm = (s.TrainingForm == null) ? null : new TrainingFormOutput
            {
                Id = s.TrainingForm.Id,
                Name = s.TrainingForm!.Name,
            },
            TrainingLevelId = s.TrainingLevelId,
            TrainingLevel = new TrainingLevelOutput
            {
                Id = s.TrainingLevel.Id,
                Name = s.TrainingLevel!.Name,
            },
            SpecializationId = s.SpecializationId,
            Specialization = (s.Specialization == null) ? null : new SpecializationOutput
            {
                Id = s.Specialization.Id,
                Name = s.Specialization!.Name,
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        AdvancedImportSelector = s =>
        {

            return null;
        };
    }

    protected override void SetOutputMapper(Thesis entity, ThesisOutput output)
    {
        output.Id = entity.Id;
        output.Name = entity.Name;
    }

    protected override void SetMapperToUpdate(ThesisInput input, Thesis entity)
    {
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.MaxStudentNumber = input.MaxStudentNumber;
        entity.Credits = input.Credits;
        entity.Year = input.Year;
        entity.TopicId = input.TopicId;
        entity.TrainingFormId = input.TrainingFormId;
        entity.TrainingLevelId = input.TrainingLevelId;
        entity.SpecializationId = input.SpecializationId;
        entity.DateFrom = input.DateFrom;
        entity.DateTo = input.DateTo;
        entity.Semester = input.Semester;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetMapperToCreate(ThesisInput input, Thesis entity)
    {
        entity.Id = UidHelper.GetShortUid();
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.MaxStudentNumber = input.MaxStudentNumber;
        entity.Credits = input.Credits;
        entity.Year = input.Year;
        entity.TopicId = input.TopicId;
        entity.TrainingFormId = input.TrainingFormId;
        entity.TrainingLevelId = input.TrainingLevelId;
        entity.SpecializationId = input.SpecializationId;
        entity.Semester = input.Semester;
        entity.CreatedAt = DateTime.Now;
    }

    public async Task<Pagination<ThesisOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Year.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Year.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(t => t.Id.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(t => t.Name.Contains(keyword)); break;
                case "Year": queryable = queryable.Where(t => t.Year.Contains(keyword)); break;
                case "LectureName": queryable = queryable.Where(t => t.Lecture.Name.Contains(keyword)); break;
                case "Semester":
                    {
                        int semester = 0;
                        if (int.TryParse(keyword, out semester))
                        {
                            queryable = queryable.Where(t => t.Semester == 1);
                        }
                        break;
                    }
            }
        }

        if (string.IsNullOrEmpty(orderBy))
        {
            queryable = queryable.OrderByDescending(f => f.CreatedAt);
        }
        else if (orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(t => t.Id); break;
                case "Name": queryable = queryable.OrderBy(t => t.Name); break;
                case "Year": queryable = queryable.OrderBy(t => t.Year); break;
                case "Semester": queryable = queryable.OrderBy(t => t.Semester); break;
                case "LectureName": queryable = queryable.OrderBy(t => t.Lecture.Name); break;
                case "CreatedAt": queryable = queryable.OrderBy(t => t.CreatedAt); break;
            }
        }
        else
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderByDescending(t => t.Id); break;
                case "Name": queryable = queryable.OrderByDescending(t => t.Name); break;
                case "Year": queryable = queryable.OrderByDescending(t => t.Year); break;
                case "Semester": queryable = queryable.OrderByDescending(t => t.Semester); break;
                case "LectureName": queryable = queryable.OrderByDescending(t => t.Lecture.Name); break;
                case "CreatedAt": queryable = queryable.OrderByDescending(t => t.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(PaginationSelector).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public override async Task<DataResponse> ImportAsync(Stream stream, ImportMetadata importMetadata)
    {
        return await _genericRepository.ImportAsync(stream, importMetadata, new ImportSelector<Thesis>
        {
            AdvancedImportSpreadsheet = AdvancedImportSelector
        });
    }

    public override async Task<ThesisOutput> GetAsync(string id)
    {
        ThesisOutput thesis = await base.GetAsync(id);

        thesis.Description = HttpUtility.HtmlDecode(thesis.Description);

        thesis.CriticalLecturer = await _context.CounterArgumentResults.Include(i => i.Lecture)
            .Where(c => c.ThesisId == id && c.IsDeleted == false)
            .Select(s => new FacultyStaffOutput { Id = s.Lecture.Id, Surname = s.Lecture.Surname, Name = s.Lecture.Name })
            .SingleOrDefaultAsync();

        thesis.ThesisSupervisor = await _context.ThesisSupervisors.Include(i => i.Lecture)
            .Where(t => t.ThesisId == id && t.IsDeleted == false)
            .Select(s => new FacultyStaffOutput { Id = s.Lecture.Id, Surname = s.Lecture.Surname, Name = s.Lecture.Name })
            .SingleOrDefaultAsync();

        return thesis;
    }

    public override Task<DataResponse<ThesisOutput>> CreateAsync(ThesisInput input)
    {
        input.Description = HttpUtility.HtmlEncode(input.Description);
        input.IsNew = true;

        return base.CreateAsync(input);
    }

    public async Task<DataResponse> RegisterThesisAsync(ThesisRegistrationInput thesisRegistrationInput)
    {
        IDbContextTransaction dbContextTransaction = null;
        try
        {
            dbContextTransaction = _context.Database.BeginTransaction();

            Thesis thesis = await _context.Theses.FindAsync(thesisRegistrationInput.ThesisId);
            if (thesis == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };

            string groupId = UidHelper.GetShortUid();
            DateTime currentDatetime = DateTime.Now;

            ThesisGroup thesisGroup = new ThesisGroup
            {
                Id = groupId,
                Name = thesisRegistrationInput.GroupName,
                Description = thesisRegistrationInput.GroupDescription,
                StudentQuantity = 0,
                CreatedAt = currentDatetime
            };

            await _context.ThesisGroups.AddAsync(thesisGroup);
            await _context.SaveChangesAsync();

            thesis.ThesisGroupId = groupId;
            await _context.SaveChangesAsync();

            List<ThesisGroupDetail> thesisGroupDetails = new List<ThesisGroupDetail>();
            string[] studentIdList = thesisRegistrationInput.StudentIdList.Split(';');

            foreach (string studentId in studentIdList)
            {
                ThesisGroupDetail thesisGroupDetail = new ThesisGroupDetail
                {
                    StudentThesisGroupId = groupId,
                    StudentId = studentId,
                    IsApproved = false,
                    CreatedAt = currentDatetime
                };

                if (studentId == thesisRegistrationInput.RegisteredStudentId)
                    thesisGroupDetail.IsLeader = true;

                thesisGroupDetails.Add(thesisGroupDetail);
            }

            await _context.ThesisGroupDetails.AddRangeAsync(thesisGroupDetails);
            await _context.SaveChangesAsync();

            await dbContextTransaction.CommitAsync();

            List<StudentOutput> students = await _context.Students.Where(s => studentIdList.Any(si => si == s.Id))
                .Select(s => new StudentOutput { Id = s.Id, Surname = s.Surname, Name = s.Name, Email = s.Email })
                .ToListAsync();

            StudentOutput registeredStudent = students
                .Find(s => s.Id == thesisRegistrationInput.RegisteredStudentId);

            StringBuilder studentListSb = new StringBuilder();
            foreach (StudentOutput student in students)
            {
                studentListSb.Append($"<p style=\"color: #000; text-align: left;\">{student.Id} - {student.FullName}</p>");
            }

            string mailContForRegdStudent = Resources.EmailResource.thesis_registered;
            mailContForRegdStudent = mailContForRegdStudent.Replace("@thesisName", thesis.Name)
                .Replace("@studentList", studentListSb.ToString());

            string mailContForMembers = Resources.EmailResource.thesis_registered_for_member;
            mailContForMembers = mailContForMembers.Replace("@thesisName", thesis.Name)
                .Replace("@registeredStudent", $"{registeredStudent.Id} - {registeredStudent.FullName}");

            await _emailService.SendAsync(
                registeredStudent.Email,
                $"Bạn đã đăng ký đề tài thành công! [{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}]",
                mailContForRegdStudent
            );

            await _emailService.SendAsync(
                students.Where(s => s.Id != registeredStudent.Id).Select(s => s.Email).ToArray(),
                $"Bạn đã được mời tham gia vào nhóm đề tài! [{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}]",
                mailContForMembers
            );

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

    public async Task<DataResponse> ApproveThesisAsync(ThesisApprovalInput approvalInput)
    {
        Thesis thesis = await _context.Theses
            .Where(t => t.Id == approvalInput.ThesisId && t.IsDeleted == false).SingleOrDefaultAsync();

        if (thesis == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy đề tài có mã này!"
            };

        thesis.Notes = approvalInput.Notes;
        thesis.IsRejected = false;
        thesis.IsApproved = true;

        await _context.SaveChangesAsync();

        string facultyStaffEmail = await _context.FacultyStaffs.Where(f => f.Id == thesis.LectureId)
            .Select(s => s.Email).SingleOrDefaultAsync();

        string mailContent = Resources.EmailResource.thesis_approved;
        mailContent = mailContent.Replace("@thesisName", thesis.Name);
        string mailSubject = $"Đề tài của bạn đã được duyệt! [{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}]";
        await _emailService.SendAsync(facultyStaffEmail, mailSubject, mailContent);

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Xét duyệt đề tài thành công!"
        };
    }

    public async Task<DataResponse> RejectThesisAsync(ThesisApprovalInput approvalInput)
    {
        Thesis thesis = await _context.Theses
            .Where(t => t.Id == approvalInput.ThesisId && t.IsDeleted == false).SingleOrDefaultAsync();

        if (thesis == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy đề tài có mã này!"
            };

        thesis.Notes = approvalInput.Notes;
        thesis.IsRejected = true;
        thesis.IsApproved = false;

        await _context.SaveChangesAsync();

        string facultyStaffEmail = await _context.FacultyStaffs.Where(f => f.Id == thesis.LectureId)
            .Select(s => s.Email).SingleOrDefaultAsync();

        string mailContent = Resources.EmailResource.thesis_rejected;
        mailContent = mailContent.Replace("@thesisName", thesis.Name).Replace("@notes", approvalInput.Notes);
        string mailSubject = $"Đề tài của bạn bị từ chối phê duyệt! [{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}]";
        await _emailService.SendAsync(facultyStaffEmail, mailSubject, mailContent);

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Từ chối xét duyệt đề tài thành công!"
        };
    }

    public async Task<DataResponse> CheckMaxStudentNumberAsync(string thesisId, int currentStudentNumber)
    {
        Thesis thesis = await _context.Theses.FindAsync(thesisId);
        if (thesis == null)
            return new DataResponse { Status = DataResponseStatus.NotFound };

        if (currentStudentNumber > thesis.MaxStudentNumber)
            return new DataResponse { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfRejectedThesis(int page, int pageSize, string keyword)
    {
        int n = (page - 1) * pageSize;
        int totalItemCount = await _context.Theses
            .Where(t => t.IsRejected == true && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .CountAsync();

        List<ThesisOutput> onePageOfData = await _context.Theses.Include(i => i.Lecture)
            .Where(t => t.IsRejected == true && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                Notes = s.Notes,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfApprovedThesis(int page, int pageSize, string keyword)
    {
        int n = (page - 1) * pageSize;
        int totalItemCount = await _context.Theses
            .Where(t => t.IsApproved == true && t.Finished == false && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .CountAsync();

        List<ThesisOutput> onePageOfData = await _context.Theses.Include(i => i.Lecture)
            .Where(t => t.IsApproved == true && t.Finished == false && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                Notes = s.Notes,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public Task<DataResponse> AllowedRegistration(string studentId, string thesisId)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResponse<string>> CheckThesisAvailable(string thesisId)
    {
        Thesis thesis = await _context.Theses.FindAsync(thesisId);
        if (thesis == null)
            return new DataResponse<string> { Status = DataResponseStatus.NotFound };

        if (string.IsNullOrEmpty(thesis.ThesisGroupId))
            return new DataResponse<string> { Status = DataResponseStatus.Success, Data = "Available" };

        return new DataResponse<string> { Status = DataResponseStatus.Success, Data = "Unavailable" };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfPublishedThesis(int page, int pageSize, string keyword)
    {
        int n = (page - 1) * pageSize;
        int totalItemCount = await _context.Theses
            .Where(t => t.IsPublished == true && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .CountAsync();

        List<ThesisOutput> onePageOfData = await _context.Theses.Include(i => i.Lecture)
            .Where(t => t.IsPublished == true && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfPublishedThesis(string lecturerId, int page, int pageSize, string keyword)
    {
        int n = (page - 1) * pageSize;
        int totalItemCount = await _context.Theses
            .Where(t => t.LectureId == lecturerId && t.IsPublished == true && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .CountAsync();

        List<ThesisOutput> onePageOfData = await _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.IsPublished == true && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfRejectedThesis(string lecturerId, int page, int pageSize, string keyword)
    {
        int n = (page - 1) * pageSize;
        int totalItemCount = await _context.Theses
            .Where(t => t.LectureId == lecturerId && t.IsRejected == true && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .CountAsync();

        List<ThesisOutput> onePageOfData = await _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.IsRejected == true && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfApprovedThesis(string lecturerId, int page, int pageSize, string keyword)
    {
        int n = (page - 1) * pageSize;
        int totalItemCount = await _context.Theses
            .Where(t => t.LectureId == lecturerId && t.IsApproved == true && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .CountAsync();

        List<ThesisOutput> onePageOfData = await _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.IsApproved == true && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                }
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public Task<Pagination<ThesisOutput>> GetPgnOf(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        throw new NotImplementedException();
    }
}
