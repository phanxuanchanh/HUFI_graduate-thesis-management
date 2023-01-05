using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class StudentThesisGroupRepository : SubRepository<StudentThesisGroup, StudentThesisGroupInput, StudentThesisGroupOutput, string>, IStudentThesisGroupRepository
{
    private HufiGraduateThesisContext _context;

    public StudentThesisGroupRepository(HufiGraduateThesisContext context)
        : base(context, context.StudentThesisGroups)
    {
        _context = context;
    }

    public async Task<DataResponse> ApprovalStudentThesisGroupAsync(string StudentThesisGroupId)
    {
        StudentThesisGroupDetail studentThesisGroupDetail = await _context.StudentThesisGroupDetails.FindAsync(StudentThesisGroupId);
        if (studentThesisGroupDetail == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy nhóm có mã này!"
            };

        studentThesisGroupDetail.IsApproved = true;
        await _context.SaveChangesAsync();
        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Bạn đã vào nhóm thành công!"
        };
    }

    public async Task<DataResponse> RefuseApprovalStudentThesisGroupAsync(string StudentThesisGroupId)
    {

        StudentThesisGroupDetail studentThesisGroupDetail = await _context.StudentThesisGroupDetails.FindAsync(StudentThesisGroupId);
        if (studentThesisGroupDetail == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy nhóm có mã này!"
            };

        studentThesisGroupDetail.IsApproved = false;
        await _context.SaveChangesAsync();
        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Bạn đã từ chối vào nhóm thành công!"
        };
    }

    protected override void ConfigureIncludes()
    {
        //_genericRepository.IncludeMany(i => i.);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new StudentThesisGroupOutput
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
        SingleSelector = s => new StudentThesisGroupOutput
        {

        };
    }
}
