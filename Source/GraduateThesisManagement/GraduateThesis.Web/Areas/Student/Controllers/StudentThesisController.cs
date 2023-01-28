using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Student.Controllers;

[Area("Student")]
[Route("student/thesis")]
[IsStudent]
[AccountInfo(typeof(StudentOutput))]
public class StudentThesisController : WebControllerBase
{
    private IAccountManager _accountManager;
    private IThesisRepository _thesisRepository;
    private IStudentRepository _studentRepository;
    private IThesisGroupRepository _thesisGroupRepository;

    public StudentThesisController(IRepository repository, IAuthorizationManager authorizationManager)
    {
        _thesisRepository = repository.ThesisRepository;
        _studentRepository = repository.StudentRepository;
        _thesisGroupRepository = repository.ThesisGroupRepository;
        _accountManager = authorizationManager.AccountManager;
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài khóa luận")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
    {
        Pagination<ThesisOutput> pagination;
        if (orderOptions == "ASC")
            pagination = await _thesisRepository.GetPgnOfPubldThesesAsync(page, pageSize, keyword);
        else
            pagination = await _thesisRepository.GetPgnOfPubldThesesAsync(page, pageSize, keyword);

        StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Xem chi tiết đề tài")]
    public async Task<IActionResult> Details([Required] string id)
    {
        ThesisOutput thesis = await _thesisRepository.GetAsync(id);
        if (thesis == null)
            return RedirectToAction("GetList");

        return View(thesis);
    }

    [Route("check-available/{id}")]
    [HttpGet]
    public async Task<IActionResult> CheckAvailable([Required] string id)
    {
        return Json(await _thesisRepository.CheckThesisAvailAsync(id));
    }

    [Route("search-students")]
    [HttpGet]
    public async Task<IActionResult> SearchStudents(string keyword)
    {   
        return Json(await _studentRepository.SearchForThesisRegAsync(keyword));
    }

    [Route("get-student-by-id")]
    [HttpGet]
    public async Task<IActionResult> GetStudentById(string studentId)
    {
        return Json(await _studentRepository.GetForThesisRegAsync(studentId));
    }

    [Route("check-max-student-number")]
    [HttpPost]
    public async Task<IActionResult> CheckMaxStudentNumber(string thesisId, int currentStudentNumber)
    {
        return Json(await _thesisRepository.CheckMaxStudentNumberAsync(thesisId, currentStudentNumber));
    }

    [Route("register/{studentId}/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Đăng ký đề tài")]
    public IActionResult Register([Required] string studentId, [Required] string thesisId)
    {
        ViewData["StudentId"] = studentId;
        ViewData["ThesisId"] = thesisId;
        return View(new ThesisRegistrationInput());
    }

    [Route("register/{studentId}/{thesisId}")]
    [HttpPost]
    [PageName(Name = "Đăng ký đề tài")]
    public async Task<IActionResult> Register(ThesisRegistrationInput thesisRegistrationInput)
    {
        if (ModelState.IsValid)
        {
            DataResponse dataResponse = await _thesisRepository.RegisterThesisAsync(thesisRegistrationInput);
            AddTempData(dataResponse);
            if(dataResponse.Status == DataResponseStatus.Success)
                return RedirectToAction("YourThesis");

            AddViewData(dataResponse);
            return View(thesisRegistrationInput);
        }

        AddTempData(DataResponseStatus.InvalidData);
        return View(thesisRegistrationInput);
    }

    [Route("join-to-group")]
    [HttpPost]
    public async Task<IActionResult> JoinToGroupAsync([Required] string groupId)
    {
        _accountManager.SetHttpContext(HttpContext);
        DataResponse dataResponse = await _thesisGroupRepository.JoinToGroupAsync(_accountManager.GetUserId(), groupId);
        AddTempData(dataResponse);

        return RedirectToAction("YourStudentThesisGroup");
    }

    [Route("deny-from-group")]
    [HttpPost]
    public async Task<IActionResult> DenyFromGroupAsync([Required] string groupId)
    {
        _accountManager.SetHttpContext(HttpContext);
        DataResponse dataResponse = await _thesisGroupRepository.DenyFromGroupAsync(_accountManager.GetUserId(), groupId);
        AddTempData(dataResponse);

        return RedirectToAction("YourStudentThesisGroup");
    }

    [Route("thesis-groups")]
    [HttpGet]
    [PageName(Name = "Danh sách nhóm đồ án của bạn")]
    public async Task<IActionResult> GetThesisGroups()
    {
        _accountManager.SetHttpContext(HttpContext);
        List<ThesisGroupDtOutput> thesisGroups = await _thesisGroupRepository
            .GetGrpsByStdntIdAsync(_accountManager.GetUserId());

        return View(thesisGroups);
    }

    [Route("my-thesis/{thesisId}")]
    [HttpPost]
    [PageName(Name = "Đề tài khóa luận của bạn")]
    public async Task<IActionResult> GetYourThesis([Required] string thesisId)
    {
        //StudentThesisOutput studentThesis = await _thesisRepository.GetStudentThesisAsync(thesisId);
        //if (studentThesis == null)
        //    return RedirectToAction("GetList");

        return View();
    }

    [Route("submit-thesis")]
    [HttpPost]
    [PageName(Name = "Nộp đề tài")]
    public async Task<IActionResult> SubmitThesisAsync([Required]string thesisId, string thesisGroupId)
    {
        DataResponse dataResponse = await _thesisRepository.SubmitThesisAsync(thesisId,thesisGroupId);
        AddTempData(dataResponse);
            return RedirectToAction("YourThesis");
    }
}
