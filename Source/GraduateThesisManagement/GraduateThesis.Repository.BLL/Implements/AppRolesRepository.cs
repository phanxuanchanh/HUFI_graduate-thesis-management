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
    public class AppRolesRepository : IAppRolesRepository
    {
        private HufiGraduateThesisContext context;
        private GenericRepository<HufiGraduateThesisContext, AppRole, AppRolesInput, AppRolesOutput> _genericRepository;

        public AppRolesRepository(HufiGraduateThesisContext context)
        {
            this.context = context;
            _genericRepository = new GenericRepository<HufiGraduateThesisContext, AppRole, AppRolesInput, AppRolesOutput>(context, context.AppRoles);

            ConfigureIncludes();
            ConfigureSelectors();
        }

        public DataResponse BatchDelete(string id)
        {
            return _genericRepository.BatchDelete(id);
        }

        public Task<DataResponse> BatchDeleteAsync(string id)
        {
            return _genericRepository.BatchDeleteAsync(id);
        }

        public void ConfigureIncludes()
        {
            _genericRepository.IncludeMany(i => i.FacultyStaffs);
        }

        public void ConfigureSelectors()
        {
            _genericRepository.PaginationSelector = s => new AppRolesOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new AppRolesOutput
            {
                Id = s.Id,
                Name = s.Name,
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

        public DataResponse<AppRolesOutput> Create(AppRolesInput input)
        {
            return _genericRepository.Create(input, GenerateUIDOptions.ShortUID);
        }

        public async Task<DataResponse<AppRolesOutput>> CreateAsync(AppRolesInput input)
        {
            return await _genericRepository.CreateAsync(input, GenerateUIDOptions.ShortUID);
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

        public AppRolesOutput Get(string id)
        {
            return _genericRepository.GetById(id);
        }

        public Task<AppRolesOutput> GetAsync(string id)
        {
            return _genericRepository.GetByIdAsync(id);
        }

        public List<AppRolesOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public Task<List<AppRolesOutput>> GetListAsync(int count = 200)
        {
            return _genericRepository.GetListAsync(count);
        }

        public Pagination<AppRolesOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<AppRolesOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }

        public DataResponse ImportFromSpreadsheet(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            return _genericRepository.ImportFromSpreadsheet(stream, spreadsheetTypeOptions, sheetName, s =>
            {
                DateTime currentDateTime = DateTime.Now;
                AppRole appRole = new AppRole
                {
                    Id = UID.GetShortUID(),
                    CreatedAt = currentDateTime
                };

                appRole.Name = s.GetCell(1).StringCellValue;
                appRole.Description = s.GetCell(2).StringCellValue;
             

                return appRole;
            });
        }

        public async Task<DataResponse> ImportFromSpreadsheetAsync(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            return _genericRepository.ImportFromSpreadsheet(stream, spreadsheetTypeOptions, sheetName, s =>
            {
                DateTime currentDateTime = DateTime.Now;
                AppRole appRole = new AppRole
                {
                    Id = UID.GetShortUID(),
                    CreatedAt = currentDateTime
                };

                appRole.Name = s.GetCell(1).StringCellValue;
                appRole.Description = s.GetCell(2).StringCellValue;


                return appRole;
            });
        }

        public DataResponse<AppRolesOutput> Update(string id, AppRolesInput input)
        {
            return _genericRepository.Update(id, input);
        }

        public async Task<DataResponse<AppRolesOutput>> UpdateAsync(string id, AppRolesInput input)
        {
            return await _genericRepository.UpdateAsync(id, input);
        }
    }
}
