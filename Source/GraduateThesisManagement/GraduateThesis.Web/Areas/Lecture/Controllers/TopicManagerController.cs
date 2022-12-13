using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using X.PagedList;
using GraduateThesis.WebExtensions;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/topic-manager")]
    public class TopicManagerController : WebControllerBase
    {
        public string PageName { get; set; } = "Quản lý chủ đề";

        private ITopicRepository _topicRepository;
        public TopicManagerController(IRepository repository)
        {
            _topicRepository = repository.TopicRepository;
        }

        [Route("list")]
        [HttpGet]
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

                AddViewData(PageName);

                return View();
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details([Required] string id)
        {
            try
            {
                TopicOutput topicOutput = await _topicRepository.GetAsync(id);
                if (topicOutput == null)
                    return RedirectToAction("Index");

                AddViewData(PageName);
                return View(topicOutput);
            }
            catch
            {
                return View(viewName: "_Error");
            }
        }

        [Route("create")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            AddViewData(PageName);
            return View();
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(TopicInput topicInput)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DataResponse<TopicOutput> dataResponse = await _topicRepository.CreateAsync(topicInput);
                    AddViewData(PageName, dataResponse);

                    return View(topicInput);
                }

                AddViewData(PageName, DataResponseStatus.InvalidData);
                return View(topicInput);
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }


    }
}
