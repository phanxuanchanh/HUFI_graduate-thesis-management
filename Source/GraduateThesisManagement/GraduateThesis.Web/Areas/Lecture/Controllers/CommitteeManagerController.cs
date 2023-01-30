using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/committee-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class CommitteeManagerController : WebControllerBase<IThesisCommitteeRepository, ThesisCommitteeInput, ThesisCommitteeOutput, string>
{
    private readonly IThesisCommitteeRepository _thesisCommitteeRepository;
    private readonly IFacultyStaffRepository _facultyStaffRepository;
    private readonly ICouncilRepository _councilRepository;

    public CommitteeManagerController(IRepository repository)
        : base(repository.ThesisCommitteeRepository)
    {
        _thesisCommitteeRepository = repository.ThesisCommitteeRepository;
        _councilRepository = repository.CouncilRepository;
        _facultyStaffRepository = repository.FacultyStaffRepository;
    }

    protected override Dictionary<string, string> SetOrderByProperties()
    {
        return new Dictionary<string, string>
        {
            { "Id", "Mã" }, { "Name", "Tên" }, { "CreatedAt", "Ngày tạo" }
        };
    }

    protected override async Task LoadSelectListAsync()
    {
        List<CouncilOutput> councils = await _councilRepository.GetListAsync(50);
        ViewData["CouncilSelectList"] = new SelectList(councils, "Id", "Name");
    }

    [Route("batch-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới tiểu ban")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới tiểu ban")]
    public override async Task<IActionResult> Create(ThesisCommitteeInput input)
    {
        return await CreateResult(input);
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết tiểu ban")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }


    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa  tiểu ban")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa  tiểu ban")]
    public override async Task<IActionResult> Edit([Required] string id, ThesisCommitteeInput input)
    {
        return await EditResult(id, input);
    }

    public override Task<IActionResult> Export()
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        throw new NotImplementedException();
    }


    [Route("force-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
    }

    [Route("trash")]
    [HttpGet]
    [PageName(Name = "Thùng rác")]
    public override async Task<IActionResult> GetTrash(int count = 50)
    {
        return await GetTrashResult(count);
    }

    public override Task<IActionResult> Import()
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> Import([FromForm, Required] IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new NotImplementedException();
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách các tiểu ban")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "CreatedAt", string orderOptions = "DESC", string keyword = "")
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    [Route("restore/{id}")]
    [HttpPost]
    public override async Task<IActionResult> Restore([Required] string id)
    {
        return await RestoreResult(id);
    }

    [Route("thesisMember/{thesisCommitteeId}")]
    [HttpGet]
    [PageName(Name = "Thêm giảng viên vào tiểu ban")]
    public async Task<IActionResult> LoadThesisMemberView(string thesisCommitteeId, int page = 1, int pageSize = 5, string keyword = "")
    {
        ThesisCommitteeOutput thesisCommittee = await _thesisCommitteeRepository.GetAsync(thesisCommitteeId);
        List<FacultyStaffOutput> facultyStaffs = await _thesisCommitteeRepository.LoadCommitteeMemberAsync(thesisCommitteeId);
        Pagination<FacultyStaffOutput> pagination = await _facultyStaffRepository
         .GetPaginationAsync(page, pageSize, null, OrderOptions.ASC, keyword);

        StaticPagedList<FacultyStaffOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;
        ViewData["Role"] = thesisCommitteeId;
        ViewData["FacultyStaffs"] = facultyStaffs;

        return View(thesisCommittee);
    }

    [Route("addMember/{MemberId}")]
    [HttpPost]
    [PageName(Name = "Phân công giảng viên vào tiểu ban")]
    public async Task<IActionResult> AddMember(CommitteeMemberInput input)
    {
        DataResponse dataResponse = await _thesisCommitteeRepository.AddMemberAsync(input);
        AddTempData(dataResponse);
        return RedirectToAction("LoadThesisMemberView", new { thesisCommitteeId = input.ThesisCommitteeId });

    }

    [Route("delete/{thesisCommitteeId}/{memberId}")]
    [HttpPost]
    [PageName(Name = "Xóa giảng viên tiểu ban")]
    public async Task<IActionResult> DeleteCommitteeMember(string thesisCommitteeId, string memberId)
    {
        DataResponse dataResponse = await _thesisCommitteeRepository.DeleteCommitteeMemberAsync(thesisCommitteeId, memberId);
        AddTempData(dataResponse);
        return RedirectToAction("LoadThesisMemberView", new { thesisCommitteeId = thesisCommitteeId });
    }
}
