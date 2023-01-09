﻿using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/faculty-manager")]
public class FacultyManagerController : WebControllerBase<IFacultyRepository, FacultyInput, FacultyOutput, string>
{
    public FacultyManagerController(IRepository repository) 
        : base(repository.FacultyRepository)
    {

    }

    [Route("batch-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> BatchDelete([Required] string id)
    {
        return await BatchDeleteResult(id);
    }

    [Route("create")]
    [HttpGet]
    [PageName(Name = "Tạo mới khoa")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới khoa")]
    public override async Task<IActionResult> Create(FacultyInput input)
    {
        return await CreateResult(input);
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết khoa")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa khoa")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa khoa")]
    public override async Task<IActionResult> Edit([Required] string id, FacultyInput input)
    {
        return await EditResult(id, input);
    }

    [Route("export")]
    public override async Task<IActionResult> Export()
    {
        return await ExportResult(null, null);
    }

    [Route("force-delete/{id}")]
    [HttpPost]
    public override async Task<IActionResult> ForceDelete([Required] string id)
    {
        return await ForceDeleteResult(id);
    }

    [Route("trash")]
    [HttpGet]
    [PageName(Name = "Thùng rác")]
    public override async Task<IActionResult> GetTrash(int count = 50)
    {
        return await GetTrashResult(count);
    }

    [Route("import")]
    [HttpGet]
    [PageName(Name = "Nhập dữ liệu vào hệ thống")]
    public override async Task<IActionResult> Import()
    {
        return await ImportResult();
    }

    [Route("import")]
    [HttpPost]
    public override async Task<IActionResult> Import([FromForm] IFormFile formFile)
    {
        return await ImportResult(formFile, new ImportMetadata());
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách khoa")]
    public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
    {
        return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
    }

    [Route("restore/{id}")]
    [HttpPost]
    public override async Task<IActionResult> Restore([Required] string id)
    {
        return await RestoreResult(id);
    }
}