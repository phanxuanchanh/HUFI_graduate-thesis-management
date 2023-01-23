﻿using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisRevisionRepository : ISubRepository<ThesisRevisionInput, ThesisRevisionOutput, string>
{
    Task<List<ThesisRevisionOutput>> GetRevsByThesisIdAsync(string thesisId);
    Task<DataResponse> ReviewRevision(ThesisRevRevisionOutput thesisRevRevision);
}