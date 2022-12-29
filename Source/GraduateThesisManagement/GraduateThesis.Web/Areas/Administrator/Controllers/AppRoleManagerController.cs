﻿using GraduateThesis.Common.Authorization;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Route("administrator/appRole-manager")]
    [HandleException]
    public class AppRoleManagerController : WebControllerBase
    {
        private IAppRolesRepository _appRolesRepository;

        public AppRoleManagerController(IRepository repository)
        {
            _appRolesRepository = repository.AppRolesRepository;
         
        }

        [Route("list")]
        [HttpGet]
        [PageName(Name = "Danh sách quyền giảng viên")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
        {

            Pagination<AppRolesOutput> pagination;
            if (orderOptions == "ASC")
                pagination = await _appRolesRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);
            else
                pagination = await _appRolesRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.DESC, keyword);

            StaticPagedList<AppRolesOutput> pagedList = pagination.ToStaticPagedList();
            ViewData["PagedList"] = pagedList;
            ViewData["OrderBy"] = orderBy;
            ViewData["OrderOptions"] = orderOptions;
            ViewData["Keyword"] = keyword;

            return View();


        }
        [Route("details/{id}")]
        [HttpGet]
        [PageName(Name = "Chi tiết quyền giảng viên")]
        public async Task<IActionResult> Details([Required] string id)
        {
            AppRolesOutput appRolesOutput = await _appRolesRepository.GetAsync(id);
            if (appRolesOutput == null)
                return RedirectToAction("Index");

            return View(appRolesOutput);
        }

        [Route("create")]
        [HttpGet]
        [PageName(Name = "Tạo mới quyền")]
        public async Task<ActionResult> Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        [PageName(Name = "Tạo mới quyền")]
        public async Task<IActionResult> Create(AppRolesInput appRolesInput)
        {
            if (ModelState.IsValid)
            {
                DataResponse<AppRolesOutput> dataResponse = await _appRolesRepository.CreateAsync(appRolesInput);
                AddViewData(dataResponse);
                return View(appRolesInput);
            }

            AddViewData(DataResponseStatus.InvalidData);
            return View(appRolesInput);
        }

        [Route("edit/{id}")]
        [HttpGet]
        [PageName(Name = "Chỉnh sửa quyền")]
        public async Task<IActionResult> Edit([Required] string id)
        {

            AppRolesOutput appRolesOutput = await _appRolesRepository.GetAsync(id);
            if (appRolesOutput == null)
                return RedirectToAction("Index");

            return View(appRolesOutput);
        }

        [Route("edit/{id}")]
        [HttpPost]
        [PageName(Name = "Chỉnh sửa đề tài khóa luận")]
        public async Task<IActionResult> Edit([Required] string id, AppRolesInput appRolesInput)
        {
           

            if (ModelState.IsValid)
            {
                AppRolesOutput appRolesOutput = await _appRolesRepository.GetAsync(id);
                if (appRolesOutput == null)
                    return RedirectToAction("Index");

                DataResponse<AppRolesOutput> dataResponse = await _appRolesRepository.UpdateAsync(id, appRolesInput);

                AddViewData(dataResponse);
                return View(appRolesInput);
            }

            AddViewData(DataResponseStatus.InvalidData);
            return View(appRolesInput);
        }

        [Route("delete/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete([Required] string id)
        {
            AppRolesOutput appRolesOutput = await _appRolesRepository.GetAsync(id);
            if (appRolesOutput == null)
                return RedirectToAction("Index");

            DataResponse dataResponse = await _appRolesRepository.BatchDeleteAsync(id);

            AddTempData(dataResponse);
            return RedirectToAction("Index");
        }

        [Route("import")]
        public IActionResult Import()
        {
            return View();
        }
    }
}
