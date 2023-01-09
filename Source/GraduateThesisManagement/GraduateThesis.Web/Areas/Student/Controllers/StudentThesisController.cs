using GraduateThesis.ApplicationCore.AppController;
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
[WebAuthorize(AccountRole.Student)]
[AccountInfo(typeof(StudentOutput))]
public class StudentThesisController : WebControllerBase
{
    private IThesisRepository _thesisRepository;
    private IStudentRepository _studentRepository;

    public StudentThesisController(IRepository repository)
    {
        _thesisRepository = repository.ThesisRepository;
        _studentRepository = repository.StudentRepository;
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách đề tài khóa luận")]
    [WebAuthorize(AccountRole.Student)]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
    {
        Pagination<ThesisOutput> pagination;
        if (orderOptions == "ASC")
            pagination = await _thesisRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);
        else
            pagination = await _thesisRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);

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
    [WebAuthorize(AccountRole.Student)]
    public async Task<IActionResult> Details([Required] string id)
    {
        ThesisOutput thesis = await _thesisRepository.GetAsync(id);
        if (thesis == null)
            return RedirectToAction("GetList");

        return View(thesis);
    }

    [Route("check-available/{id}")]
    [HttpGet]
    [WebAuthorize(AccountRole.Student)]
    public async Task<IActionResult> CheckAvailable([Required] string id)
    {
        ThesisOutput thesis = await _thesisRepository.GetAsync(id);
        if (thesis == null)
            return Json(new DataResponse<ThesisOutput> { Status = DataResponseStatus.NotFound });

        if(string.IsNullOrEmpty(thesis.ThesisGroupId))
            return Json(new DataResponse<string> { Status = DataResponseStatus.Success, Data = "Available" });

        return Json(new DataResponse<string> { Status = DataResponseStatus.Success, Data = "Unavailable" });
    }

    [Route("search-students")]
    [HttpGet]
    [WebAuthorize(AccountRole.Student)]
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
    [WebAuthorize(AccountRole.Student)]
    public async Task<IActionResult> CheckMaxStudentNumber(string thesisId, int currentStudentNumber)
    {
        return Json(await _thesisRepository.CheckMaxStudentNumberAsync(thesisId, currentStudentNumber));
    }

    [Route("register/{studentId}/{thesisId}")]
    [HttpGet]
    [PageName(Name = "Đăng ký đề tài")]
    [WebAuthorize(AccountRole.Student)]
    public IActionResult Register([Required] string studentId, [Required] string thesisId)
    {
        ViewData["StudentId"] = studentId;
        ViewData["ThesisId"] = thesisId;
        return View(new ThesisRegistrationInput());
    }

    [Route("register/{studentId}/{thesisId}")]
    [HttpPost]
    [PageName(Name = "Đăng ký đề tài")]
    [WebAuthorize(AccountRole.Student)]
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

        AddViewData(DataResponseStatus.InvalidData);
        return View(thesisRegistrationInput);
    }

    [Route("my-thesis/{thesisId}")]
    [HttpPost]
    [PageName(Name = "Đề tài khóa luận của bạn")]
    [WebAuthorize(AccountRole.Student)]
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
    [WebAuthorize(AccountRole.Student)]
    public async Task<IActionResult> SubmitThesisAsync([Required]string thesisId, string thesisGroupId)
    {
        DataResponse dataResponse = await _thesisRepository.SubmitThesisAsync(thesisId,thesisGroupId);
        AddTempData(dataResponse);
            return RedirectToAction("YourThesis");
    }
}
