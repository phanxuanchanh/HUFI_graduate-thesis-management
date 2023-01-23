using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.AppController;
using X.PagedList;
using GraduateThesis.WebExtensions;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Authorization;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/thesis-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class ThesisManagerController : WebControllerBase<IThesisRepository, ThesisInput, ThesisOutput, string>
{
    private readonly IAccountManager _accountManager;
    private readonly ITopicRepository _topicRepository;
    private readonly ITrainingFormRepository _trainingFormRepository;
    private readonly ITrainingLevelRepository _trainingLevelRepository;
    private readonly IThesisRepository _thesisRepository;
    private readonly IThesisRevisionRepository _thesisRevisionRepository;
    private readonly ISpecializationRepository _specializationRepository;

    public ThesisManagerController(IRepository repository, IAuthorizationManager authorizationManager)
        : base(repository.ThesisRepository)
    {
        _thesisRepository = repository.ThesisRepository;
        _thesisRevisionRepository = repository.ThesisRevisionRepository;
        _trainingFormRepository = repository.TrainingFormRepository;
        _trainingLevelRepository = repository.TrainingLevelRepository;
        _topicRepository = repository.TopicRepository;
        _specializationRepository = repository.SpecializationRepository;
        _accountManager = authorizationManager.AccountManager;
    }

    protected override async Task LoadSelectListAsync()
    {
        List<TopicOutput> topics = await _topicRepository.GetListAsync(50);
        ViewData["TopicSelectList"] = new SelectList(topics, "Id", "Name");

        List<TrainingLevelOutput> trainingLevels = await _trainingLevelRepository.GetListAsync(50);
        ViewData["TrainingLevelSelectList"] = new SelectList(trainingLevels, "Id", "Name");

        List<TrainingFormOutput> trainingForms = await _trainingFormRepository.GetListAsync(50);
        ViewData["TrainingFormSelectList"] = new SelectList(trainingForms, "Id", "Name");

        List<SpecializationOutput> specializations = await _specializationRepository.GetListAsync(50);
        ViewData["SpecializationsSelectList"] = new SelectList(specializations, "Id", "Name");
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách các đề tài khóa luận")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết đề tài khóa luận")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới đề tài khóa luận")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới đề tài khóa luận")]
    public override async Task<IActionResult> Create(ThesisInput thesisInput)
    {
        return await CreateResult(thesisInput);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa đề tài khóa luận")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa đề tài khóa luận")]
    public override async Task<IActionResult> Edit([Required] string id, ThesisInput thesisInput)
    {
        return await EditResult(id, thesisInput);
    }

    [Route("batch-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    [Route("force-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
    }

    public override async Task<IActionResult> Export()
    {
        return await ExportResult(null!, null!);
    }

    [Route("export")]
    [HttpPost]
    public override Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        throw new NotImplementedException();
    }

    [Route("import")]
    [HttpPost]
    [PageName(Name = "Nhập dữ liệu vào hệ thống")]
    public override async Task<IActionResult> Import([Required][FromForm] IFormFile formFile, ImportMetadata importMetadata)
    {
        return await ImportResult(formFile, importMetadata);
    }

    [Route("import")]
    [HttpGet]
    [PageName(Name = "Nhập dữ liệu vào hệ thống")]
    public override async Task<IActionResult> Import()
    {
        return await ImportResult();
    }

    [Route("trash")]
    [HttpGet]
    [PageName(Name = "Thùng rác")]
    public override async Task<IActionResult> GetTrash(int count = 50)
    {
        return await GetTrashResult(count);
    }

    [Route("restore/{id}")]
    [HttpPost]
    public override async Task<IActionResult> Restore([Required] string id)
    {
        return await RestoreResult(id);
    }

    [Route("approve-thesis/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Xét duyệt đề tài")]
    public async Task<IActionResult> ApproveThesis([Required] string thesisId)
    {
        ThesisOutput thesis = await _thesisRepository.GetAsync(thesisId);
        if (thesis == null)
            return NotFound();

        if (thesis.IsApproved)
            return RedirectToAction("GetRejectedList");

        ViewData["Thesis"] = thesis;

        return View(new ThesisApprovalInput { ThesisId = thesis.Id });
    }

    [Route("approve-thesis/{thesisId}")]
    [HttpPost]
    [PageName(Name = "Xét duyệt đề tài")]
    public async Task<IActionResult> ApproveThesis(ThesisApprovalInput approvalInput)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("ApproveThesis", new { thesisId = approvalInput.ThesisId });
        }

        DataResponse dataResponse = await _thesisRepository.ApproveThesisAsync(approvalInput);
        AddTempData(dataResponse);

        return RedirectToAction("ApproveThesis", new { thesisId = approvalInput.ThesisId });
    }

    [Route("reject-thesis/{thesisId}")]
    [HttpPost]
    [PageName(Name = "Từ chối xét duyệt đề tài")]
    public async Task<IActionResult> RejectThesis(ThesisApprovalInput approvalInput)
    {
        DataResponse dataResponse = await _thesisRepository.RejectThesisAsync(approvalInput);
        AddTempData(dataResponse);

        return RedirectToAction("ApproveThesis", new { thesisId = approvalInput.ThesisId });
    }

    [Route("approved-list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài đã được duyệt")]
    public async Task<IActionResult> GetApprovedList(int page = 1, int pageSize = 10, string keyword = "")
    {
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfApprovedThesis(page, pageSize, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("rejected-list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài bị từ chối xét duyệt")]
    public async Task<IActionResult> GetRejectedList(int page = 1, int pageSize = 10, string keyword = "")
    {
        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfRejectedThesis(page, pageSize, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("my-thesis")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài của tôi")]
    public async Task<IActionResult> GetThesisOfLecturer(int page = 1, int pageSize = 10, string keyword = "")
    {
        _accountManager.SetHttpContext(HttpContext);
        string userId = _accountManager.GetUserId();

        Pagination<ThesisOutput> pagination = await _thesisRepository.GetPgnOfApprovedThesis(userId, page, pageSize, keyword);
        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();

        ViewData["PagedList"] = pagedList;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("revisions/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Tiến độ của đề tài")]
    public async Task<IActionResult> GetRevisions(string thesisId)
    {
        return View(await _thesisRevisionRepository.GetRevsByThesisIdAsync(thesisId));
    }

    [Route("review-revision/{revisionId}")]
    [HttpGet]
    [PageName(Name = "Nhận xét về bản thay đổi")]
    public async Task<IActionResult> ReviewRevision(string revisionId)
    {
        ThesisRevisionOutput thesisRevision = await _thesisRevisionRepository.GetAsync(revisionId);
        if (thesisRevision == null)
            return NotFound();

        if (thesisRevision.Reviewed)
            return RedirectToAction("GetRevisions", new { thesisId = thesisRevision.Thesis.Id });

        ViewData["ThesisRevision"] = thesisRevision;

        return View(new ThesisRevReviewInput { RevisionId = thesisRevision.Id });
    }

    [Route("review-revision/{revisionId}")]
    [HttpPost]
    [PageName(Name = "Nhận xét về bản thay đổi")]
    public async Task<IActionResult> ReviewRevision(ThesisRevReviewInput thesisRevReview)
    {
        ThesisRevisionOutput thesisRevision = await _thesisRevisionRepository.GetAsync(thesisRevReview.RevisionId);
        if (!ModelState.IsValid)
        {
            AddViewData(DataResponseStatus.InvalidData);
            ViewData["ThesisRevision"] = thesisRevision;

            return View(new ThesisRevReviewInput { RevisionId = thesisRevision.Id });
        }

        DataResponse dataResponse = await _thesisRevisionRepository.ReviewRevisionAsync(thesisRevReview);
        AddTempData(dataResponse);

        return RedirectToAction("GetRevisions", new { thesisId = thesisRevision.Thesis.Id });
    }
}