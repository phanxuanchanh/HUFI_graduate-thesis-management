﻿using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Implements;

public class AppRolesRepository : SubRepository<AppRole, AppRolesInput, AppRolesOutput, string>, IAppRolesRepository
{
    private HufiGraduateThesisContext _context;

    public AppRolesRepository(HufiGraduateThesisContext context)
        :base(context, context.AppRoles)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        _genericRepository.IncludeMany(i => i.FacultyStaffs);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new AppRolesOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new AppRolesOutput
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            DeletedAt = s.DeletedAt
        };
    }
}
