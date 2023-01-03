﻿using GraduateThesis.ApplicationCore.ApiAttributes;
using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GraduateThesis.WebApi.Controllers;

[Route("api/thesis-committee")]
[ApiController]
[ApiAuthorize()]
public class ThesisCommitteeControler : ApiControllerBase<IThesisCommitteeRepository, ThesisCommitteeInput, ThesisCommitteeOutput, string>
{
    public ThesisCommitteeControler(IRepository repository) : base(repository.ThesisCommitteeRepository)
    {
    }

    public override Task<IActionResult> BatchDelete([Required] string id)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Create(ThesisCommitteeInput input)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Export(ExportMetadata exportMetadata)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> ForceDelete([Required] string id)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> GetDetails([Required] string id)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> GetPagination(Pagination<ThesisCommitteeOutput> pagination)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Import(IFormFile formFile, ImportMetadata importMetadata)
    {
        throw new System.NotImplementedException();
    }

    public override Task<IActionResult> Update([Required] string id, ThesisCommitteeInput input)
    {
        throw new System.NotImplementedException();
    }
}

