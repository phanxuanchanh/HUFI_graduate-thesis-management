using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Administrator.Controllers;

[Area("Administrator")]
[Route("admin/app-role-manager")]
public class AppRoleManagerController : WebControllerBase
{
    private IAppRoleRepository _appRolesRepository;

    public AppRoleManagerController(IRepository repository)
    {
        _appRolesRepository = repository.AppRolesRepository;
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách quyền")]
    public async Task<IActionResult> Index(string keyword = "")
    {
        return View();
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết quyền")]
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
}
