﻿using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface ITrainingLevelRepository : ISubRepository<TrainingLevelInput, TrainingLevelOutput, string>
{
}
