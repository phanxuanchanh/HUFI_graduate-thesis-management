﻿using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/training-level-manager")]
[WebAuthorize]
[AccountInfo(typeof(FacultyStaffOutput))]
public class TrainingLevelManagerController : WebControllerBase<ITrainingLevelRepository, TrainingLevelInput, TrainingLevelOutput, string>
{
    public TrainingLevelManagerController(IRepository repository) 
        : base(repository.TrainingLevelRepository)
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
    [PageName(Name = "Tạo mới bậc đào tạo")]
    public override async Task<IActionResult> Create()
    {
        return await CreateResult();
    }

    [Route("create")]
    [HttpPost]
    [PageName(Name = "Tạo mới bậc đào tạo")]
    public override async Task<IActionResult> Create(TrainingLevelInput input)
    {
        return await CreateResult(input);
    }

    [Route("details/{id}")]
    [HttpGet]
    [PageName(Name = "Chi tiết bậc đào tạo")]
    public override async Task<IActionResult> Details([Required] string id)
    {
        return await GetDetailsResult(id);
    }

    [Route("edit/{id}")]
    [HttpGet]
    [PageName(Name = "Chỉnh sửa bậc đào tạo")]
    public override async Task<IActionResult> Edit([Required] string id)
    {
        return await EditResult(id);
    }

    [Route("edit/{id}")]
    [HttpPost]
    [PageName(Name = "Chỉnh sửa bậc đào tạo")]
    public override async Task<IActionResult> Edit([Required] string id, TrainingLevelInput input)
    {
        return await EditResult(id, input);
    }

    [NonAction]
    public override Task<IActionResult> Export()
    {
        throw new NotImplementedException();
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

    [NonAction]
    public override Task<IActionResult> Import()
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override Task<IActionResult> Import([FromForm] IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new NotImplementedException();
    }

    [Route("list")]
    [HttpGet]
    [PageName(Name = "Danh sách bậc đào tạo")]
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
