using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using GraduateThesis.WebExtensions;
using System.ComponentModel.DataAnnotations;
using GraduateThesis.Common.WebAttributes;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/topic-manager")]
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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = null, string orderOptions = "ASC", string keyword = null)
        {
            try
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
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("details/{id}")]
        [HttpGet]
        [PageName(Name = "Chi tiết chủ đề khóa luận")]
        public async Task<IActionResult> Details([Required] string id)
        {
            try
            {
                TopicOutput topicOutput = await _topicRepository.GetAsync(id);
                if (topicOutput == null)
                    return RedirectToAction("Index");
;
                return View(topicOutput);
            }
            catch
            {
                return View(viewName: "_Error");
            }
        }

        [Route("create")]
        [HttpGet]
        [PageName(Name = "Tạo mới chủ đề khóa luận")]
        public ActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        [PageName(Name = "Tạo mới chủ đề khóa luận")]
        public async Task<IActionResult> Create(TopicInput topicInput)
        {
            try
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
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }
        [Route("edit/{id}")]
        [HttpGet]
        [PageName(Name = "Chỉnh sửa chủ đề khóa luận")]
        public async Task<IActionResult> Edit([Required] string id)
        {
            try
            {            
                TopicOutput topicOutput = await _topicRepository.GetAsync(id);
                if (topicOutput == null)
                    return RedirectToAction("Index");

                return View(topicOutput);
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("edit/{id}")]
        [HttpPost]
        [PageName(Name = "Chỉnh sửa chủ đề khóa luận")]
        public async Task<IActionResult> Edit([Required] string id, TopicInput topicInput)
        {
            try
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
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("delete/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete([Required] string id)
        {
            try
            {
                TopicOutput topicOutput = await _topicRepository.GetAsync(id);
                if (topicOutput == null)
                    return RedirectToAction("Index");

                DataResponse dataResponse = await _topicRepository.BatchDeleteAsync(id);

                AddTempData(dataResponse);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }


    }
}
