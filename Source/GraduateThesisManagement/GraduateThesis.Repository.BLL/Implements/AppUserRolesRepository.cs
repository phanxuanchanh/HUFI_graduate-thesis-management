using GraduateThesis.Common;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements
{
    public class AppUserRolesRepository : IAppUserRolesRepository
    {
        private HufiGraduateThesisContext context;
        private GenericRepository<HufiGraduateThesisContext, AppUserRole, AppUserRoleInput, AppUserRoleOutput> _genericRepository;

        public AppUserRolesRepository(HufiGraduateThesisContext context)
        {
            this.context = context;
            _genericRepository = new GenericRepository<HufiGraduateThesisContext, AppUserRole, AppUserRoleInput, AppUserRoleOutput>(context, context.AppUserRoles);

            ConfigureIncludes();
            ConfigureSelectors();
        }

        public DataResponse BatchDelete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse> BatchDeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public void ConfigureIncludes()
        {
            _genericRepository.IncludeMany(i => i.Role, i=>i.User) ;
        }

        public void ConfigureSelectors()
        {
            _genericRepository.PaginationSelector = s => new AppUserRoleOutput
            {
                UserId = s.UserId,
                RoleId = s.RoleId,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new AppUserRoleOutput
            {
                UserId = s.UserId,
                RoleId = s.RoleId,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public DataResponse<AppUserRoleOutput> Create(AppUserRoleInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<AppUserRoleOutput>> CreateAsync(AppUserRoleInput input)
        {
            throw new NotImplementedException();
        }

        public IWorkbook ExportToSpreadsheet(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IWorkbook> ExportToSpreadsheetAsync(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public DataResponse ForceDelete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse> ForceDeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public AppUserRoleOutput Get(string id)
        {
            return _genericRepository.GetById(id);
        }

        public Task<AppUserRoleOutput> GetAsync(string id)
        {
            return _genericRepository.GetByIdAsync(id);
        }

        public List<AppUserRoleOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public Task<List<AppUserRoleOutput>> GetListAsync(int count = 200)
        {
            return _genericRepository.GetListAsync(count);
        }

        public Pagination<AppUserRoleOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<AppUserRoleOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }


        public DataResponse ImportFromSpreadsheet(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            return _genericRepository.ImportFromSpreadsheet(stream, spreadsheetTypeOptions, sheetName, s =>
            {
                DateTime currentDateTime = DateTime.Now;
                AppUserRole appUserRole = new AppUserRole
                {
                    UserId = UID.GetShortUID(),
                    CreatedAt = currentDateTime
                };

                appUserRole.RoleId = s.GetCell(1).StringCellValue;
                appUserRole.RoleId = s.GetCell(2).StringCellValue;

                return appUserRole;
            });
        }

        public async Task<DataResponse> ImportFromSpreadsheetAsync(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {

            return await _genericRepository.ImportFromSpreadsheetAsync(stream, spreadsheetTypeOptions, sheetName, s =>
            {
                DateTime currentDateTime = DateTime.Now;
                AppUserRole appUserRole = new AppUserRole
                {
                    UserId = UID.GetShortUID(),
                    CreatedAt = currentDateTime
                };

                appUserRole.RoleId = s.GetCell(1).StringCellValue;
                appUserRole.RoleId = s.GetCell(2).StringCellValue;

                return appUserRole;
            });
        }


        public DataResponse<AppUserRoleOutput> Update(string id, AppUserRoleInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<AppUserRoleOutput>> UpdateAsync(string id, AppUserRoleInput input)
        {
            throw new NotImplementedException();
        }
    }
}
