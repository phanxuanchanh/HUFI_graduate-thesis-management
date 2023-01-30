using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.ExtensionMethods;
using GraduateThesis.Repository.BLL.Consts;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MiniExcelLibs;
using NPOI.SS.Formula.Functions;
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
    private IHostingEnvironment _hostingEnvironment;

    internal ThesisRepository(HufiGraduateThesisContext context, IHostingEnvironment hostingEnvironment, IEmailService emailService)
        : base(context, context.Theses)
    {
        _context = context;
        _emailService = emailService;
        _hostingEnvironment = hostingEnvironment;
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
            Year = s.Year,
            Semester = s.Semester,
            Lecturer = new FacultyStaffOutput
            {
                Id = s.Lecture.Id,
                Surname = s.Lecture.Surname,
                Name = s.Lecture.Name
            },
            ThesisStatus = new ThesisStatusOutput
            {
                Id = s.Status.Id,
                Name = s.Status.Name
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
        entity.LectureId = input.LectureId;
        entity.StatusId = input.StatusId;
        entity.CreatedAt = DateTime.Now;
    }

    private IQueryable<Thesis> SearchByFullName(IQueryable<Thesis> queryable, string fullName)
    {
        int lastIndexOf = fullName.Trim(' ').LastIndexOf(" ");
        if (lastIndexOf > 0)
        {
            string name = fullName.Substring(lastIndexOf).Trim(' ');
            string surname = fullName.Replace(name, "").Trim(' ');
            queryable = queryable.Where(t => t.Lecture.Surname.Contains(surname) && t.Lecture.Name.Contains(name));
        }
        else
        {
            queryable = queryable.Where(t => t.Lecture.Name.Contains(fullName));
        }

        return queryable;
    }

    private IQueryable<Thesis> SearchBySemester(IQueryable<Thesis> queryable, string semsterString)
    {
        int semester = 0;
        if (int.TryParse(semsterString, out semester))
            queryable = queryable.Where(t => t.Semester == semester);

        return queryable;
    }

    private IQueryable<Thesis> SetSearchExpression(IQueryable<Thesis> queryable, string searchBy, string keyword)
    {
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
                case "LectureName": queryable = SearchByFullName(queryable, keyword); break;
                case "Semester": queryable = SearchBySemester(queryable, keyword); break;
            }
        }

        return queryable;
    }

    private IQueryable<Thesis> SetOrderExpression(IQueryable<Thesis> queryable, string orderBy, OrderOptions orderOptions)
    {
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

        return queryable;
    }

    public async Task<Pagination<ThesisOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

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

        thesis.CriticalLecturer = await _context.CounterArgumentResults.Include(i => i.Lecturer)
            .Where(c => c.ThesisId == id && c.Lecturer.IsDeleted == false)
            .Select(s => new FacultyStaffOutput { 
                Id = s.Lecturer.Id, 
                Surname = s.Lecturer.Surname, 
                Name = s.Lecturer.Name,
                Email = s.Lecturer.Email
            }).SingleOrDefaultAsync();

        thesis.ThesisSupervisor = await _context.ThesisSupervisors.Include(i => i.Lecturer)
            .Where(ts => ts.ThesisId == id && ts.Lecturer.IsDeleted == false)
            .Select(s => new FacultyStaffOutput { 
                Id = s.Lecturer.Id, 
                Surname = s.Lecturer.Surname, 
                Name = s.Lecturer.Name,
                Email = s.Lecturer.Email
            }).SingleOrDefaultAsync();

        return thesis;
    }

    public override Task<DataResponse<ThesisOutput>> CreateAsync(ThesisInput input)
    {
        input.Description = HttpUtility.HtmlEncode(input.Description);
        input.StatusId = ThesisStatusConsts.Pending;

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

        thesis.StatusId = ThesisStatusConsts.Finished;
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
            .Where(t => t.Id == approvalInput.ThesisId && t.IsDeleted == false)
            .SingleOrDefaultAsync();

        if (thesis == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy đề tài có mã này!"
            };

        if(thesis.StatusId >= ThesisStatusConsts.Approved)
            return new DataResponse
            {
                Status = DataResponseStatus.Failed,
                Message = "Không thể thực hiện lại việc xét duyệt đề tài đã được duyệt!"
            };

        thesis.Notes = approvalInput.Notes;
        thesis.StatusId = ThesisStatusConsts.Approved;

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

        if(thesis.StatusId >= ThesisStatusConsts.Approved)
            return new DataResponse
            {
                Status = DataResponseStatus.Failed,
                Message = "Không thể từ chối xét duyệt khi đề tài đã được duyệt!"
            };

        thesis.Notes = approvalInput.Notes;
        thesis.StatusId = ThesisStatusConsts.Rejected;

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

    public async Task<Pagination<ThesisOutput>> GetPgnOfPndgApvlThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
                    .Where(t => t.StatusId == ThesisStatusConsts.Pending && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

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

    public async Task<Pagination<ThesisOutput>> GetPgnOfPndgApvlThesesAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Pending && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

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

    public async Task<Pagination<ThesisOutput>> GetPgnOfRejdThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
                    .Where(t => t.StatusId == ThesisStatusConsts.Rejected && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                Year = s.Year,
                Semester = s.Semester,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                },
                Notes = s.Notes
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfAppdThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.StatusId == ThesisStatusConsts.Approved && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

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

    public Task<DataResponse> AllowedRegistration(string studentId, string thesisId)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResponse<string>> CheckThesisAvailAsync(string thesisId)
    {
        Thesis thesis = await _context.Theses.FindAsync(thesisId);
        if (thesis == null)
            return new DataResponse<string> { Status = DataResponseStatus.NotFound };

        if (string.IsNullOrEmpty(thesis.ThesisGroupId))
            return new DataResponse<string> { Status = DataResponseStatus.Success, Data = "Available" };

        return new DataResponse<string> { Status = DataResponseStatus.Success, Data = "Unavailable" };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfPubldThesesAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.StatusId == ThesisStatusConsts.Published && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
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

    public async Task<Pagination<ThesisOutput>> GetPgnOfPubldThesesAsync(string lecturerId, int page, int pageSize, string keyword)
    {
        int n = (page - 1) * pageSize;
        int totalItemCount = await _context.Theses
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Published && t.IsDeleted == false)
            .Where(t => t.Id.Contains(keyword) || t.Name.Contains(keyword) || t.Description.Contains(keyword))
            .CountAsync();

        List<ThesisOutput> onePageOfData = await _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.StatusId == 4 && t.IsDeleted == false)
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

    public async Task<Pagination<ThesisOutput>> GetPgnOfRejdThesesAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Rejected && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                Year = s.Year,
                Semester = s.Semester,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                },
                Notes = s.Notes
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfAppdThesesAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Approved && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

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

    public async Task<DataResponse> AssignSupervisorAsync(string thesisId, string lecturerId)
    {
        bool checkThesisExists = await _context.Theses
          .AnyAsync(t => t.Id == thesisId && t.IsDeleted == false);

        if (!checkThesisExists)
            return new DataResponse { 
                Status = DataResponseStatus.NotFound, 
                Message = "Không tìm thấy đề tài này!" 
            };

        bool checkSupvExists = await _context.FacultyStaffs
            .AnyAsync(f => f.Id == lecturerId && f.IsDeleted == false);

        if(!checkSupvExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy giảng viên này!"
            };

        bool checkAsgmtExists = await _context.ThesisSupervisors
            .AnyAsync(ts => ts.ThesisId == thesisId);

        if (checkAsgmtExists)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Đề tài này đã được phân công, cần hủy phân công hiện tại trước khi phân công lại!"
            };

        ThesisSupervisor thesisSupervisor = new ThesisSupervisor
        {
            ThesisId = thesisId,
            LecturerId = lecturerId
        };

        await _context.ThesisSupervisors.AddAsync(thesisSupervisor);
        await _context.SaveChangesAsync();

        return new DataResponse { Status = DataResponseStatus.Success, Message = "Phân công giảng viên cho đề tài thành công!" };
    }

    public async Task<DataResponse> AssignSupervisorsAsync(string[] thesisIds)
    {
        List<Thesis> theses = await _context.Theses.Include(i => i.Lecture)
            .Where(t => t.IsDeleted == false && t.Lecture.IsDeleted == false)
            .WhereBulkContains(thesisIds, t => t.Id)
            .ToListAsync();

        List<ThesisSupervisor> thesisSupervisors = new List<ThesisSupervisor>();
        foreach(Thesis thesis in theses)
        {
            thesisSupervisors.Add(new ThesisSupervisor { ThesisId = thesis.Id, LecturerId = thesis.LectureId });
        }

        await _context.ThesisSupervisors.AddRangeAsync(thesisSupervisors);
        await _context.SaveChangesAsync();

        return new DataResponse { 
            Status = DataResponseStatus.Success, 
            Message = "Hoàn tất phân công giảng viên hướng dẫn cho những đề tài đã chọn!" 
        };
    }

    public async Task<DataResponse> AssignSupervisorAsync(string thesisId)
    {
        Thesis thesis = await _context.Theses.Where(t => t.Id == thesisId && t.IsDeleted == false).SingleOrDefaultAsync();
        if (thesis == null)
            return new DataResponse { 
                Status = DataResponseStatus.NotFound, 
                Message = "Không tìm thấy đề tài này!" 
            };

        bool checkAsgmtExists = await _context.ThesisSupervisors
            .AnyAsync(ts => ts.ThesisId == thesisId);

        if (checkAsgmtExists)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Đề tài này đã được phân công, cần hủy phân công hiện tại trước khi phân công lại!"
            };

        ThesisSupervisor thesisSupervisor = new ThesisSupervisor
        {
            ThesisId = thesisId,
            LecturerId = thesis.LectureId
        };

        await _context.ThesisSupervisors.AddAsync(thesisSupervisor);
        await _context.SaveChangesAsync();

        return new DataResponse { 
            Status = DataResponseStatus.Success, 
            Message = "Phân công giảng viên cho đề tài thành công!" 
        };

    }

    public async Task<Pagination<ThesisOutput>> GetPaginationAsync(string lecturerId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.LectureId == lecturerId && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

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

    public async Task<DataResponse> AssignCLectAsync(string thesisId, string lecturerId)
    {
        bool checkExists = await _context.Theses
            .AnyAsync(t => t.Id == thesisId && t.IsDeleted == false);

        if (!checkExists)
            return new DataResponse { 
                Status = DataResponseStatus.NotFound, 
                Message = "Không có đề tài này!" 
            };

        bool checkSupervisorExists = await _context.ThesisSupervisors
            .AllAsync(ts => ts.ThesisId == thesisId && ts.LecturerId == lecturerId);

        if (checkSupervisorExists)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Không thể phân công cho giảng viên này, vì giảng viên này đang đảm nhận vai trò GVHD"
            };

        bool checkAsgmtExists = await _context.CounterArgumentResults
            .AnyAsync(c => c.ThesisId == thesisId);

        if (checkAsgmtExists)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Đề tài này đã được phân công, cần hủy phân công hiện tại trước khi phân công lại!"
            };

        CounterArgumentResult counterArgument = new CounterArgumentResult
        {
            ThesisId = thesisId,
            LecturerId = lecturerId
        };

        await _context.CounterArgumentResults.AddAsync(counterArgument);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Phân công giảng viên phản biện cho đề tài thành công!"
        };
    }

    public async Task<byte[]> ExportAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses
                .Include(i => i.Topic).Include(i => i.Specialization).Include(i => i.Lecture)
                .Where(t => t.IsDeleted == false && t.Lecture.IsDeleted == false)
                .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Name = "DANH SÁCH ĐỀ TÀI KHÓA LUẬN - KHOA CNTT",
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name,
                    LectureName = $"{s.Lecture.Surname.Trim(' ')} {s.Lecture.Name.Trim(' ')}"
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportPndgThesesAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "pending-thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses
                .Include(i => i.Topic).Include(i => i.Specialization).Include(i => i.Lecture)
                .Where(t => t.StatusId == ThesisStatusConsts.Pending && t.IsDeleted == false && t.Lecture.IsDeleted == false)
                .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Name = "DANH SÁCH ĐỀ TÀI ĐANG CHỜ DUYỆT - KHOA CNTT",
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name,
                    LectureName = $"{s.Lecture.Surname.Trim(' ')} {s.Lecture.Name.Trim(' ')}"
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportPubldThesesAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "published-thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses
                .Include(i => i.Topic).Include(i => i.Specialization).Include(i => i.Lecture)
                .Where(t => t.StatusId == ThesisStatusConsts.Published && t.IsDeleted == false)
                .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name,
                    LectureName = $"{s.Lecture.Surname.Trim(' ')} {s.Lecture.Name.Trim(' ')}"
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportRejdThesesAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "rejected-thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses
                .Include(i => i.Topic).Include(i => i.Specialization).Include(i => i.Lecture)
                .Where(t => t.StatusId == ThesisStatusConsts.Rejected && t.IsDeleted == false && t.Lecture.IsDeleted == false)
                .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Name = "DANH SÁCH ĐỀ TÀI ĐÃ BỊ TỪ CHỐI DUYỆT - KHOA CNTT",
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name,
                    LectureName = $"{s.Lecture.Surname.Trim(' ')} {s.Lecture.Name.Trim(' ')}",
                    Notes = s.Notes
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportAppdThesesAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses.Include(i => i.Topic).Include(i => i.Specialization)
            .Where(t => t.StatusId == ThesisStatusConsts.Approved && t.IsDeleted == false)
            .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportAsync(string lecturerId)
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses.Include(i => i.Topic).Include(i => i.Specialization)
            .Where(t => t.LectureId == lecturerId && t.IsDeleted == false)
            .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportPubldThesesAsync(string lecturerId)
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses.Include(i => i.Topic).Include(i => i.Specialization)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Published && t.IsDeleted == false)
            .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportRejdThesesAsync(string lecturerId)
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses.Include(i => i.Topic).Include(i => i.Specialization)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Rejected && t.IsDeleted == false)
            .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportAppdThesesAsync(string lecturerId)
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses.Include(i => i.Topic).Include(i => i.Specialization)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Approved && t.IsDeleted == false)
            .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<DataResponse> PublishThesisAsync(string thesisId)
    {
        Thesis thesis = await _context.Theses.Where(t => t.Id == thesisId && t.IsDeleted == false)
            .SingleOrDefaultAsync();

        if (thesis == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy đề tài có mã này!"
            };

        if(thesis.StatusId < ThesisStatusConsts.Approved)
            return new DataResponse
            {
                Status = DataResponseStatus.Failed,
                Message = "Không thể công bố đề tài vì đề tài chưa được duyệt!"
            };

        if(thesis.StatusId >= ThesisStatusConsts.InProgress)
            return new DataResponse
            {
                Status = DataResponseStatus.Failed,
                Message = "Không thể công bố khi đề tài đang diễn ra hoặc đã kết thúc!"
            };

        thesis.StatusId = ThesisStatusConsts.Published;
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã công bố đề tài thành công!"
        };
    }

    public async Task<DataResponse> PublishThesesAsync(string[] thesisIds)
    {
        List<Thesis> theses = await _context.Theses
            .Where(t => t.StatusId == ThesisStatusConsts.Approved && t.IsDeleted == false)
            .WhereBulkContains(thesisIds, s => s.Id).ToListAsync();

        theses.ForEach((t) => { t.StatusId = ThesisStatusConsts.Published; });
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã công bố đề tài thành công!"
        };
    }

    public async Task<DataResponse> StopPubgThesisAsync(string thesisId)
    {
        Thesis thesis = await _context.Theses
            .Where(t => t.Id == thesisId && t.IsDeleted == false).SingleOrDefaultAsync();

        if (thesis == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy đề tài có mã này!"
            };

        if(thesis.StatusId != ThesisStatusConsts.Published)
            return new DataResponse
            {
                Status = DataResponseStatus.Failed,
                Message = "Không thể ngừng công bố khi đề tài chưa công bố!"
            };

        thesis.StatusId = ThesisStatusConsts.InProgress;
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã ngừng công bố đề tài thành công!"
        };
    }

    public async Task<DataResponse> StopPubgThesesAsync(string[] thesisIds)
    {
        List<Thesis> theses = await _context.Theses
            .Where(t => t.StatusId == ThesisStatusConsts.Published && t.IsDeleted == false)
            .WhereBulkContains(thesisIds, s => s.Id).ToListAsync();

        theses.ForEach((t) => { t.StatusId = ThesisStatusConsts.InProgress; });
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã ngừng công bố đề tài thành công!"
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfThesesInprAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => t.StatusId == ThesisStatusConsts.InProgress && t.IsDeleted == false && t.Lecture.IsDeleted == false);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
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

    public async Task<Pagination<ThesisOutput>> GetPgnToAssignSupvAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        List<string> thesisIds = await _context.ThesisSupervisors.Include(i => i.Thesis)
            .Where(ts => (ts.Thesis.StatusId == ThesisStatusConsts.Published || ts.Thesis.StatusId == ThesisStatusConsts.InProgress) && ts.Thesis.IsDeleted == false)
            .Select(s => s.Thesis.Id).ToListAsync();

        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => (t.StatusId == ThesisStatusConsts.Published || t.StatusId == ThesisStatusConsts.InProgress) && t.ThesisGroupId != null && t.IsDeleted == false && t.Lecture.IsDeleted == false)
            .WhereBulkNotContains(thesisIds, t => t.Id);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Semester = s.Semester,
                Year = s.Year,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Lecture.Id,
                    Surname = s.Lecture.Surname,
                    Name = s.Lecture.Name
                },
            }).ToListAsync();

        return new Pagination<ThesisOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<DataResponse> RemoveAssignSupvAsync(string thesisId, string lecturerId)
    {
        ThesisSupervisor thesisSupervisor = await _context.ThesisSupervisors.FindAsync(thesisId);
        if (thesisSupervisor == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy thông tin phân công giảng viên này!"
            };

        if (thesisSupervisor.LecturerId != lecturerId)
            return new DataResponse
            {
                Status = DataResponseStatus.Failed,
                Message = "Mã giảng viên không khớp!"
            };

        _context.ThesisSupervisors.Remove(thesisSupervisor);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Hoàn tất hủy phân công!"
        };
    }

    public async Task<Pagination<ThesisOutput>> GetPgnOfAssignedSupvAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<ThesisSupervisor> queryable = _context.ThesisSupervisors
            .Include(i => i.Thesis).Include(i => i.Thesis.Lecture)
            .Where(ts => (ts.Thesis.StatusId == ThesisStatusConsts.Published || ts.Thesis.StatusId == ThesisStatusConsts.InProgress) && ts.Thesis.IsDeleted == false && ts.Thesis.Lecture.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(t => t.Thesis.Id.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(t => t.Thesis.Name.Contains(keyword)); break;
                case "Year": queryable = queryable.Where(t => t.Thesis.Year.Contains(keyword)); break;
                case "LectureName":
                    {
                        int lastIndexOf = keyword.Trim(' ').LastIndexOf(" ");
                        if (lastIndexOf > 0)
                        {
                            string name = keyword.Substring(lastIndexOf).Trim(' ');
                            string surname = keyword.Replace(name, "").Trim(' ');
                            queryable = queryable.Where(t => t.Thesis.Lecture.Surname.Contains(surname) && t.Thesis.Lecture.Name.Contains(name));
                        }
                        else
                        {
                            queryable = queryable.Where(t => t.Thesis.Lecture.Name.Contains(keyword));
                        }
                        break;
                    }
                case "Semester":
                    {
                        int semester = 0;
                        if (int.TryParse(keyword, out semester))
                            queryable = queryable.Where(t => t.Thesis.Semester == semester);
                        
                        break;
                    }
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Thesis.Id,
                Name = s.Thesis.Name,
                MaxStudentNumber = s.Thesis.MaxStudentNumber,
                ThesisGroupId = s.Thesis.ThesisGroupId,
                Semester = s.Thesis.Semester,
                Year = s.Thesis.Year,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Thesis.Lecture.Id,
                    Surname = s.Thesis.Lecture.Surname,
                    Name = s.Thesis.Lecture.Name
                },
                ThesisSupervisor = new FacultyStaffOutput
                {
                    Id = s.Lecturer.Id,
                    Surname = s.Lecturer.Surname,
                    Name = s.Lecturer.Name
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

    public async Task<Pagination<ThesisOutput>> GetPgnToAssignCLectAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        List<string> thesisIds = await _context.CounterArgumentResults.Include(i => i.Thesis)
            .Where(ts => (ts.Thesis.StatusId == ThesisStatusConsts.Published || ts.Thesis.StatusId == ThesisStatusConsts.InProgress) && ts.Thesis.IsDeleted == false)
            .Select(s => s.Thesis.Id).ToListAsync();

        IQueryable<Thesis> queryable = _context.Theses.Include(i => i.Lecture)
            .Where(t => (t.StatusId == ThesisStatusConsts.Published || t.StatusId == ThesisStatusConsts.InProgress) && t.ThesisGroupId != null && t.IsDeleted == false && t.Lecture.IsDeleted == false)
            .WhereBulkNotContains(thesisIds, t => t.Id);

        queryable = SetSearchExpression(queryable, searchBy, keyword);
        queryable = SetOrderExpression(queryable, orderBy, orderOptions);

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId,
                Semester = s.Semester,
                Year = s.Year,
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

    public async Task<Pagination<ThesisOutput>> GetPgnOfAssignedCLectAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<CounterArgumentResult> queryable = _context.CounterArgumentResults
            .Include(i => i.Thesis).Include(i => i.Thesis.Lecture)
            .Where(ts => (ts.Thesis.StatusId == ThesisStatusConsts.Published || ts.Thesis.StatusId == ThesisStatusConsts.InProgress) && ts.Thesis.IsDeleted == false && ts.Thesis.Lecture.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(ts => ts.Thesis.Id.Contains(keyword) || ts.Thesis.Name.Contains(keyword) || ts.Thesis.Year.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(t => t.Thesis.Id.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(t => t.Thesis.Name.Contains(keyword)); break;
                case "Year": queryable = queryable.Where(t => t.Thesis.Year.Contains(keyword)); break;
                case "LectureName":
                    {
                        int lastIndexOf = keyword.Trim(' ').LastIndexOf(" ");
                        if (lastIndexOf > 0)
                        {
                            string name = keyword.Substring(lastIndexOf).Trim(' ');
                            string surname = keyword.Replace(name, "").Trim(' ');
                            queryable = queryable.Where(t => t.Thesis.Lecture.Surname.Contains(surname) && t.Thesis.Lecture.Name.Contains(name));
                        }
                        else
                        {
                            queryable = queryable.Where(t => t.Thesis.Lecture.Name.Contains(keyword));
                        }
                        break;
                    }
                case "Semester":
                    {
                        int semester = 0;
                        if (int.TryParse(keyword, out semester))
                            queryable = queryable.Where(t => t.Thesis.Semester == semester);

                        break;
                    }
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<ThesisOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new ThesisOutput
            {
                Id = s.Thesis.Id,
                Name = s.Thesis.Name,
                MaxStudentNumber = s.Thesis.MaxStudentNumber,
                ThesisGroupId = s.Thesis.ThesisGroupId,
                Semester = s.Thesis.Semester,
                Year = s.Thesis.Year,
                Lecturer = new FacultyStaffOutput
                {
                    Id = s.Thesis.Lecture.Id,
                    Surname = s.Thesis.Lecture.Surname,
                    Name = s.Thesis.Lecture.Name
                },
                CriticalLecturer = new FacultyStaffOutput
                {
                    Id = s.Lecturer.Id,
                    Surname = s.Lecturer.Surname,
                    Name = s.Lecturer.Name
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

    public async Task<DataResponse> RemoveAssignCLectAsync(string thesisId, string lecturerId)
    {
        CounterArgumentResult counterArgument = await _context.CounterArgumentResults.FindAsync(thesisId);
        if (counterArgument == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy thông tin phân công giảng viên này!"
            };

        if (counterArgument.LecturerId != lecturerId)
            return new DataResponse
            {
                Status = DataResponseStatus.Failed,
                Message = "Mã giảng viên không khớp!"
            };

        _context.CounterArgumentResults.Remove(counterArgument);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Hoàn tất hủy phân công!"
        };
    }
}
