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
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements
{
    public class FacultyRepository : IFacultyRepository
    {
        private HufiGraduateThesisContext context;
        private GenericRepository<HufiGraduateThesisContext, Faculty, FacultyInput, FacultyOutput> _genericRepository;

        public FacultyRepository(HufiGraduateThesisContext context)
        {
            this.context = context;
            _genericRepository = new GenericRepository<HufiGraduateThesisContext, Faculty, FacultyInput, FacultyOutput>(context, context.Faculties);

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
            _genericRepository.PaginationSelector = s => new FacultyOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new FacultyOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                FacultyStaffs = s.FacultyStaffs.Select(f => new FacultyStaffOutput
                {
                    Id = f.Id,
                    FullName = f.FullName
                }).ToList(),
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };
        }

        public int Count()
        {
            return _genericRepository.Count();
        }

        public Task<int> CountAsync()
        {
            return _genericRepository.CountAsync();
        }

        public DataResponse<FacultyOutput> Create(FacultyInput input)
        {
            return _genericRepository.Create(input, GenerateUIDOptions.ShortUID);
        }

        public async Task<DataResponse<FacultyOutput>> CreateAsync(FacultyInput input)
        {
            return await _genericRepository.CreateAsync(input, GenerateUIDOptions.ShortUID);
        }

        public IWorkbook ExportToSpreadsheet(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            return _genericRepository.ExportToSpreadsheet(spreadsheetTypeOptions, sheetName, includeProperties);
        }

        public async Task<IWorkbook> ExportToSpreadsheetAsync(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            return await _genericRepository
                .ExportToSpreadsheetAsync(spreadsheetTypeOptions, sheetName, includeProperties);
        }

        public DataResponse ForceDelete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse> ForceDeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public FacultyOutput Get(string id)
        {
            return _genericRepository.Get("Id", id);
        }

        public Task<FacultyOutput> GetAsync(string id)
        {
            return _genericRepository.GetByIdAsync(id);
        }

        public List<FacultyOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public Task<List<FacultyOutput>> GetListAsync(int count = 200)
        {
            return _genericRepository.GetListAsync(count);
        }

        public Pagination<FacultyOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<FacultyOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }

        public DataResponse ImportFromSpreadsheet(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse> ImportFromSpreadsheetAsync(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            throw new NotImplementedException();
        }

        public DataResponse<FacultyOutput> Update(string id, FacultyInput input)
        {
            return _genericRepository.Update(id, input);
        }

        public Task<DataResponse<FacultyOutput>> UpdateAsync(string id, FacultyInput input)
        {
            return _genericRepository.UpdateAsync(id, input);
        }
    }
}
