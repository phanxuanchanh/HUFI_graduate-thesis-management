using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements
{
    public class FacultyStaffRoleRepository : IFacultyStaffRoleRepository
    {
        private HufiGraduateThesisContext context;
        private GenericRepository<HufiGraduateThesisContext, FacultyStaffRole, FacultyStaffRoleInput, FacultyStaffRoleOutput> _genericRepository;

        public FacultyStaffRoleRepository(HufiGraduateThesisContext context)
        {
            this.context = context;
            _genericRepository = new GenericRepository<HufiGraduateThesisContext, FacultyStaffRole, FacultyStaffRoleInput, FacultyStaffRoleOutput>(context, context.FacultyStaffRoles);

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
            _genericRepository.IncludeMany(i => i.FacultyStaffs);
        }

        public void ConfigureSelectors()
        {
            _genericRepository.PaginationSelector = s => new FacultyStaffRoleOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new FacultyStaffRoleOutput
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

        public DataResponse<FacultyStaffRoleOutput> Create(FacultyStaffRoleInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<FacultyStaffRoleOutput>> CreateAsync(FacultyStaffRoleInput input)
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

        public FacultyStaffRoleOutput Get(string id)
        {
            return _genericRepository.GetById(id);
        }

        public Task<FacultyStaffRoleOutput> GetAsync(string id)
        {
            return _genericRepository.GetByIdAsync(id);
        }

        public List<FacultyStaffRoleOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public Task<List<FacultyStaffRoleOutput>> GetListAsync(int count = 200)
        {
            return _genericRepository.GetListAsync(count);
        }

        public Pagination<FacultyStaffRoleOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<FacultyStaffRoleOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }


        public DataResponse<FacultyStaffRoleOutput> Update(string id, FacultyStaffRoleInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<FacultyStaffRoleOutput>> UpdateAsync(string id, FacultyStaffRoleInput input)
        {
            throw new NotImplementedException();
        }
    }
}
