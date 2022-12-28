using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using GraduateThesis.WebExtensions;
using System.ComponentModel.DataAnnotations;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Common.File;
using NPOI.SS.UserModel;
using System.Net.Mime;
using GraduateThesis.Common.Authorization;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/topic-manager")]
    [WebAuthorize(AccountRole.Lecture)]
    [AccountInfo(typeof(FacultyStaffOutput))]
    [HandleException]
    public class TopicManagerController : WebControllerBase
    {
        private ITopicRepository _topicRepository;
        public TopicManagerController(IRepository repository)
        {
            _topicRepository = repository.TopicRepository;
        }

        [Route("list")]
        [HttpGet]
        [PageName(Name = "Danh sách các chủ đề khóa luận")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
        {
            Pagination<TopicOutput> pagination;
            if (orderOptions == "ASC")
                pagination = await _topicRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);
            else
                pagination = await _topicRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.DESC, keyword);

            StaticPagedList<TopicOutput> pagedList = pagination.ToStaticPagedList();
            ViewData["PagedList"] = pagedList;
            ViewData["OrderBy"] = orderBy;
            ViewData["OrderOptions"] = orderOptions;
            ViewData["Keyword"] = keyword;

            return View();
        }

        [Route("details/{id}")]
        [HttpGet]
        [PageName(Name = "Chi tiết chủ đề khóa luận")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Details([Required] string id)
        {
            TopicOutput topicOutput = await _topicRepository.GetAsync(id);
            if (topicOutput == null)
                return RedirectToAction("Index");
            ;
            return View(topicOutput);
        }

        [Route("create")]
        [HttpGet]
        [PageName(Name = "Tạo mới chủ đề khóa luận")]
        [WebAuthorize(AccountRole.Lecture)]
        public ActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        [PageName(Name = "Tạo mới chủ đề khóa luận")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Create(TopicInput topicInput)
        {
            if (ModelState.IsValid)
            {
                DataResponse<TopicOutput> dataResponse = await _topicRepository.CreateAsync(topicInput);
                AddViewData(dataResponse);

                return View(topicInput);
            }

            AddViewData(DataResponseStatus.InvalidData);
            return View(topicInput);
        }

        [Route("edit/{id}")]
        [HttpGet]
        [PageName(Name = "Chỉnh sửa chủ đề khóa luận")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Edit([Required] string id)
        {
            TopicOutput topicOutput = await _topicRepository.GetAsync(id);
            if (topicOutput == null)
                return RedirectToAction("Index");

            return View(topicOutput);
        }

        [Route("edit/{id}")]
        [HttpPost]
        [PageName(Name = "Chỉnh sửa chủ đề khóa luận")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Edit([Required] string id, TopicInput topicInput)
        {
            if (ModelState.IsValid)
            {
                TopicOutput topicOutput = await _topicRepository.GetAsync(id);
                if (topicOutput == null)
                    return RedirectToAction("Index");

                DataResponse<TopicOutput> dataResponse = await _topicRepository.UpdateAsync(id, topicInput);

                AddViewData(dataResponse);
                return View(topicInput);
            }

            AddViewData(DataResponseStatus.InvalidData);
            return View(topicInput);
        }

        [Route("delete/{id}")]
        [HttpPost]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Delete([Required] string id)
        {
            TopicOutput topicOutput = await _topicRepository.GetAsync(id);
            if (topicOutput == null)
                return RedirectToAction("Index");

            DataResponse dataResponse = await _topicRepository.BatchDeleteAsync(id);

            AddTempData(dataResponse);
            return RedirectToAction("Index");
        }

        [Route("export-to-spreadsheet")]
        [HttpGet]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> ExportToSpreadsheet()
        {
            IWorkbook workbook = await _topicRepository.ExportToSpreadsheetAsync(
                SpreadsheetTypeOptions.XLSX,
                "Danh sách đề tài",
                new string[] { "Id", "Name" }
            );

            ContentDisposition contentDisposition = new ContentDisposition
            {
                FileName = $"Thesis_{DateTime.Now.ToString("ddMMyyyy_hhmmss")}.xlsx",
                Inline = true,
            };

            Response.Headers.Append("Content-Disposition", contentDisposition.ToString());

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.Write(memoryStream, true);
                byte[] bytes = memoryStream.ToArray();
                return File(bytes, ContentTypeConsts.XLSX);
            }
        }
    }
}
