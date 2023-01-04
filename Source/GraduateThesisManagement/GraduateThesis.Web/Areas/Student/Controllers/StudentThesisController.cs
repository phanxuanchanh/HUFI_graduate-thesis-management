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

namespace GraduateThesis.Web.Areas.Student.Controllers
{
    [Area("Student")]
    [Route("student/thesis")]
    [WebAuthorize(AccountRole.Student)]
    [AccountInfo(typeof(StudentOutput))]
    public class StudentThesisController : WebControllerBase
    {
        private IThesisRepository _thesisRepository;

        public StudentThesisController(IRepository repository)
        {
            _thesisRepository = repository.ThesisRepository;
        }

        [Route("overview")]
        [HttpGet]
        [PageName(Name = "Trang tổng quan")]
        [WebAuthorize(AccountRole.Student)]
        public IActionResult Index()
        {
            return View();
        }

        [Route("list")]
        [HttpGet]
        [PageName(Name = "Danh sách đề tài khóa luận")]
        [WebAuthorize(AccountRole.Student)]
        public async Task<IActionResult> GetList(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
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

        [Route("check-thesis-available/{id}")]
        [HttpGet]
        [WebAuthorize(AccountRole.Student)]
        public async Task<IActionResult> CheckThesisAvailable([Required] string id)
        {
            ThesisOutput thesis = await _thesisRepository.GetAsync(id);
            if (thesis == null)
                return Json(new DataResponse<ThesisOutput> { Status = DataResponseStatus.NotFound });

            if(string.IsNullOrEmpty(thesis.ThesisGroupId))
                return Json(new DataResponse<string> { Status = DataResponseStatus.Success, Data = "Available" });

            return Json(new DataResponse<string> { Status = DataResponseStatus.Success, Data = "Unavailable" });
        }

        [Route("do-thesis-register/{studentId}/{thesisId}")]
        [HttpGet]
        [PageName(Name = "Đăng ký đề tài")]
        [WebAuthorize(AccountRole.Student)]
        public IActionResult DoThesisRegister([Required] string studentId, [Required] string thesisId)
        {
            return View(new ThesisRegisterInput());
        }

        [Route("do-thesis-register/{studentId}/{thesisId}")]
        [HttpPost]
        [PageName(Name = "Đăng ký đề tài")]
        [WebAuthorize(AccountRole.Student)]
        public async Task<IActionResult> DoThesisRegister(ThesisRegisterInput thesisRegisterInput)
        {
            if (ModelState.IsValid)
            {
                DataResponse dataResponse = await _thesisRepository.DoThesisRegisterAsync(thesisRegisterInput);
                AddTempData(dataResponse);
                if(dataResponse.Status == DataResponseStatus.Success)
                    return RedirectToAction("YourThesis");

                AddViewData(dataResponse);
                return View(thesisRegisterInput);
            }

            AddViewData(DataResponseStatus.InvalidData);
            return View(thesisRegisterInput);
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
            return View();
        }

        
    }
}
