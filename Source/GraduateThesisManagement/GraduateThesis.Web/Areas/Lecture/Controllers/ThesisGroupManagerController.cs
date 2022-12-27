using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using NuGet.Protocol.Core.Types;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/thesisgroup-manager")]
    public class ThesisGroupManagerController : WebControllerBase
    {
        private IStudentThesisGroupRepository _studentThesisGroupRepository;
   
        public ThesisGroupManagerController(IRepository repository)
        {
            _studentThesisGroupRepository = repository.StudentThesisGroupRepository;
           
        }

        [Route("list")]
        [HttpGet]
        [PageName(Name = "Danh sách nhóm sinh viên làm khóa luận")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
        {
            try
            {
                

                Pagination<StudentThesisGroupOutput> pagination;
                if (orderOptions == "ASC")
                    pagination = await _studentThesisGroupRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);
                else
                    pagination = await _studentThesisGroupRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.DESC, keyword);

                StaticPagedList<StudentThesisGroupOutput> pagedList = pagination.ToStaticPagedList();
                ViewData["PagedList"] = pagedList;
                ViewData["OrderBy"] = orderBy;
                ViewData["OrderOptions"] = orderOptions;
                ViewData["Keyword"] = keyword;

                return View();
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("create")]
        [HttpGet]
        [PageName(Name = "Tạo mới nhóm đề tài khóa luận")]
        public async Task<ActionResult> Create()
        {

            return View();
        }

        [Route("create")]
        [HttpPost]
        [PageName(Name = "Tạo mới đề tài khóa luận")]
        public async Task<IActionResult> Create(StudentThesisGroupInput studentThesisGroupInput)
        {
            try
            {
                

                if (ModelState.IsValid)
                {
                    DataResponse<StudentThesisGroupOutput> dataResponse = await _studentThesisGroupRepository.CreateAsync(studentThesisGroupInput);
                    AddViewData(dataResponse);

                    return View(studentThesisGroupInput);
                }

                AddViewData(DataResponseStatus.InvalidData);
                return View(studentThesisGroupInput);
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

    }
}
