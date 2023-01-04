﻿using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ApplicationCore.WebAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

#nullable disable

namespace GraduateThesis.ApplicationCore.AppController;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>

[HandleException]
public class WebControllerBase : Controller
{
    [NonAction]
    protected void AddViewData(DataResponse dataResponse)
    {
        ViewData["Status"] = dataResponse.Status.ToString();

        if (string.IsNullOrEmpty(dataResponse.Message))
        {
            if (dataResponse.Status == DataResponseStatus.Success)
                ViewData["Message"] = "";
            else if (dataResponse.Status == DataResponseStatus.NotFound)
                ViewData["Message"] = "";
            else if (dataResponse.Status == DataResponseStatus.AlreadyExists)
                ViewData["Message"] = "";
            else
                ViewData["Message"] = "";
        }
        else
        {
            ViewData["Message"] = dataResponse.Message;
        }
    }

    [NonAction]
    protected void AddViewData(DataResponseStatus dataResponseStatus)
    {
        ViewData["Status"] = dataResponseStatus.ToString();

        if (dataResponseStatus == DataResponseStatus.Success)
            ViewData["Message"] = "";
        else if (dataResponseStatus == DataResponseStatus.NotFound)
            ViewData["Message"] = "";
        else if (dataResponseStatus == DataResponseStatus.AlreadyExists)
            ViewData["Message"] = "";
        else
            ViewData["Message"] = "";
    }

    [NonAction]
    protected void AddViewData(SignInResultModel signInResultModel)
    {
        ViewData["Status"] = signInResultModel.Status.ToString();

        if (string.IsNullOrEmpty(signInResultModel.Message))
        {
            if (signInResultModel.Status == SignInStatus.Success)
                ViewData["Message"] = "";
            else if (signInResultModel.Status == SignInStatus.WrongPassword)
                ViewData["Message"] = "";
            else if (signInResultModel.Status == SignInStatus.NotFound)
                ViewData["Message"] = "";
            else
                ViewData["Message"] = "";
        }
        else
        {
            ViewData["Message"] = signInResultModel.Message;
        }
    }

    [NonAction]
    protected void AddTempData(SignInStatus signInStatus)
    {
        TempData["Status"] = signInStatus.ToString();

        if (signInStatus == SignInStatus.Success)
            TempData["Message"] = "";
        else if (signInStatus == SignInStatus.WrongPassword)
            TempData["Message"] = "";
        else if (signInStatus == SignInStatus.NotFound)
            TempData["Message"] = "";
        else
            TempData["Message"] = "";
    }

    [NonAction]
    protected void AddTempData(SignInResultModel signInResultModel)
    {
        TempData["Status"] = signInResultModel.Status.ToString();

        if (string.IsNullOrEmpty(signInResultModel.Message))
        {
            if (signInResultModel.Status == SignInStatus.Success)
                TempData["Message"] = "";
            else if (signInResultModel.Status == SignInStatus.WrongPassword)
                TempData["Message"] = "";
            else if (signInResultModel.Status == SignInStatus.NotFound)
                TempData["Message"] = "";
            else
                TempData["Message"] = "";
        }
        else
        {
            TempData["Message"] = signInResultModel.Message;
        }
    }

    [NonAction]
    protected void AddTempData(DataResponse dataResponse)
    {
        TempData["Status"] = dataResponse.Status.ToString();

        if (string.IsNullOrEmpty(dataResponse.Message))
        {
            if (dataResponse.Status == DataResponseStatus.Success)
                TempData["Message"] = "";
            else if (dataResponse.Status == DataResponseStatus.NotFound)
                TempData["Message"] = "";
            else if (dataResponse.Status == DataResponseStatus.AlreadyExists)
                TempData["Message"] = "";
            else
                TempData["Message"] = "";
        }
        else
        {
            TempData["Message"] = dataResponse.Message;
        }
    }

}


/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>
/// <typeparam name="TSubRepository"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
/// <typeparam name="T_ID"></typeparam>

public abstract class WebControllerBase<TSubRepository, TInput, TOutput, T_ID> : WebControllerBase
    where TSubRepository : class
    where TInput : class
    where TOutput : class
{
    private readonly ISubRepository<TInput, TOutput, T_ID> _subRepository;

    public WebControllerBase(TSubRepository subRepository)
    {
        _subRepository = (subRepository as ISubRepository<TInput, TOutput, T_ID>);
    }

    [NonAction]
    protected async Task<IActionResult> IndexResult(int page, int pageSize, string orderBy, string orderOptions, string keyword)
    {
        Pagination<TOutput> paginationInput = new Pagination<TOutput>
        {
            Page = page,
            PageSize = pageSize,
            OrderBy = orderBy,
            OrderOptions = (orderOptions == "ASC") ? OrderOptions.ASC : OrderOptions.DESC,
            SearchKeyword = keyword
        };

        Pagination<TOutput> pagination = await _subRepository.GetPaginationAsync(paginationInput);
        StaticPagedList<TOutput> pagedList = new StaticPagedList<TOutput>(pagination.Items, page, pageSize, pagination.TotalItemCount);
        
        ViewData["PagedList"] = pagedList;
        ViewData["OrderBy"] = orderBy;
        ViewData["OrderOptions"] = orderOptions;
        ViewData["Keyword"] = keyword;

        return View();
    }

    [NonAction]
    protected async Task<IActionResult> GetDetailsResult(T_ID id)
    {
        TOutput output = await _subRepository.GetAsync(id);
        if (output == null)
            return RedirectToAction("Index");

        return View(output);
    }

    [NonAction]
    protected async Task<IActionResult> CreateResult()
    {
        return await Task.Run(() =>
        {
            return View();
        });
    }

    [NonAction]
    protected async Task<IActionResult> CreateResult(Func<Task> loadDependency)
    {
        await loadDependency();
        return View();
    }

    [NonAction]
    protected async Task<IActionResult> CreateResult(TInput input)
    {
        if (ModelState.IsValid)
        {
            DataResponse<TOutput> dataResponse = await _subRepository.CreateAsync(input);
            AddViewData(dataResponse);

            return View(input);
        }

        AddViewData(DataResponseStatus.InvalidData);
        return View(input);
    }

    [NonAction]
    protected async Task<IActionResult> CreateResult(TInput input, Func<Task> loadDependency)
    {
        await loadDependency();
        if (ModelState.IsValid)
        {
            DataResponse<TOutput> dataResponse = await _subRepository.CreateAsync(input);
            AddViewData(dataResponse);

            return View(input);
        }

        AddViewData(DataResponseStatus.InvalidData);
        return View(input);
    }

    [NonAction]
    protected async Task<IActionResult> EditResult(T_ID id)
    {
        TOutput output = await _subRepository.GetAsync(id);
        if (output == null)
            return RedirectToAction("Index");

        return View(output);
    }

    [NonAction]
    protected async Task<IActionResult> EditResult(T_ID id, Func<Task> loadDependency)
    {
        TOutput output = await _subRepository.GetAsync(id);
        if (output == null)
            return RedirectToAction("Index");

        await loadDependency();

        return View(output);
    }

    [NonAction]
    protected async Task<IActionResult> EditResult(T_ID id, TInput input)
    {
        if (ModelState.IsValid)
        {
            DataResponse<TOutput> dataResponse = await _subRepository.UpdateAsync(id, input);
            AddViewData(dataResponse);
            return View(input);
        }

        AddViewData(DataResponseStatus.InvalidData);
        return View(input);
    }

    [NonAction]
    protected async Task<IActionResult> BatchDeleteResult([Required] T_ID id)
    {
        TOutput output = await _subRepository.GetAsync(id);
        if (output == null)
            return RedirectToAction("Index");

        DataResponse dataResponse = await _subRepository.BatchDeleteAsync(id);
        AddTempData(dataResponse);

        return RedirectToAction("Index");
    }

    [NonAction]
    protected async Task<IActionResult> ForceDeleteResult([Required] T_ID id)
    {
        TOutput output = await _subRepository.GetAsync(id);
        if (output == null)
            return RedirectToAction("Index");

        DataResponse dataResponse = await _subRepository.ForceDeleteAsync(id);
        AddTempData(dataResponse);

        return RedirectToAction("Index");
    }

    [NonAction]
    protected async Task<IActionResult> ExportResult(RecordFilter recordFilter, ExportMetadata exportMetadata)
    {
        return View(await _subRepository.ExportAsync(recordFilter, exportMetadata));
    }

    [NonAction]
    protected async Task<IActionResult> ImportResult(IFormFile formFile, ImportMetadata importMetadata)
    {
        MemoryStream memoryStream = null;
        try
        {
            DataResponse dataResponse = await _subRepository.ImportAsync(memoryStream, importMetadata);
            AddTempData(dataResponse);

            return View();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    //public abstract Task<IActionResult> Index(Pagination<TOutput> pagination);
    public abstract Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "");
    public abstract Task<IActionResult> Details([Required] T_ID id);
    public abstract Task<IActionResult> Create();
    public abstract Task<IActionResult> Create(TInput input);
    public abstract Task<IActionResult> Edit([Required] T_ID id);
    public abstract Task<IActionResult> Edit([Required] T_ID id, TInput input);
    public abstract Task<IActionResult> BatchDelete([Required] T_ID id);
    public abstract Task<IActionResult> ForceDelete([Required] T_ID id);
    public abstract Task<IActionResult> Export();
    public abstract Task<IActionResult> Import(IFormFile formFile);
}