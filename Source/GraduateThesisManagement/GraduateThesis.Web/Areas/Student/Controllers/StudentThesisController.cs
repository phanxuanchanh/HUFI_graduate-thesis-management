using GraduateThesis.Common.Authorization;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Student.Controllers
{
    [Area("Student")]
    [Route("student/thesis")]
    [WebAuthorize(AccountRole.Student)]
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
        public async Task<IActionResult> GetList(int page = 1, int pageSize = 10, string orderBy = null, string orderOptions = null, string keyword = null)
        {
            try
            {
                Pagination<ThesisOutput> pagination;
                if (orderOptions == "ASC")
                    pagination = await _thesisRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);
                else
                    pagination = await _thesisRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);

                StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();
                ViewData["PagedList"] = pagedList;

                return View();
            }
            catch(Exception)
            {
                return View(viewName: "_Error");
            }
            
        }

        [Route("details/{id}")]
        [HttpGet]
        [PageName(Name = "Xem chi tiết đề tài")]
        [WebAuthorize(AccountRole.Student)]
        public async Task<IActionResult> Details([Required] string id)
        {
            try
            {
                ThesisOutput thesis = await _thesisRepository.GetAsync(id);
                if (thesis == null)
                    return RedirectToAction("GetList");

                return View(thesis);
            }
            catch (Exception)
            {
                return View(viewName: "_Error");
            }
        }

        [Route("do-thesis-register/{studentId}/{thesisId}")]
        [HttpPost]
        [PageName(Name = "Đăng ký đề tài")]
        [WebAuthorize(AccountRole.Student)]
        public IActionResult DoThesisRegister([Required] string studentId, [Required] string thesisId)
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View(viewName: "_Error");
            }
        }

        [Route("submit-thesis")]
        [HttpPost]
        [PageName(Name = "Nộp đề tài")]
        [WebAuthorize(AccountRole.Student)]
        public IActionResult SubmitThesis()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View(viewName: "_Error");
            }
        }
    }
}
