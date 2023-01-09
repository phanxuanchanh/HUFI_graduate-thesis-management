using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
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

        };
    }

    public async Task<DataResponse> ApprovalStudentThesisGroupAsync(string StudentThesisGroupId)
    {
        ThesisGroupDetail thesisGroupDetail = await _context.ThesisGroupDetails.FindAsync(StudentThesisGroupId);
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

    public async Task<DataResponse> RefuseApprovalStudentThesisGroupAsync(string StudentThesisGroupId)
    {

        ThesisGroupDetail thesisGroupDetail = await _context.ThesisGroupDetails.FindAsync(StudentThesisGroupId);
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
            Message = "Bạn đã từ chối vào nhóm thành công!"
        };
    }
}
