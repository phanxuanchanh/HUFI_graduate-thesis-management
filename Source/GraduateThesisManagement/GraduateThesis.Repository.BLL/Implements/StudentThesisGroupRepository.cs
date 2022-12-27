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
    public class StudentThesisGroupRepository : IStudentThesisGroupRepository
    {
        private HufiGraduateThesisContext _context;
        private GenericRepository<HufiGraduateThesisContext, StudentThesisGroup, StudentThesisGroupInput, StudentThesisGroupOutput> _genericRepository;

        public StudentThesisGroupRepository(HufiGraduateThesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HufiGraduateThesisContext, StudentThesisGroup, StudentThesisGroupInput, StudentThesisGroupOutput>(context, context.StudentThesisGroups);

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
            //_genericRepository.IncludeMany(i => i.);
        }

        public void ConfigureSelectors()
        {
            _genericRepository.PaginationSelector = s => new StudentThesisGroupOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                StudentQuantity = s.StudentQuantity,
                Notes = s.Notes,
                Thesis = s.Theses.Select(ts=> new ThesisOutput
                {
                    Id = ts.Id,
                    Name = ts.Name,
                    Description = ts.Description,
                    SourceCode = ts.SourceCode,
                    Notes = ts.Notes,
                    TopicId = ts.Notes,
                    MaxStudentNumber = ts.MaxStudentNumber,
                   
                }).FirstOrDefault()
                

            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new StudentThesisGroupOutput
            {

            };
        }

        public int Count()
        {
            return _genericRepository.Count();
        }

        public async Task<int> CountAsync()
        {
            return await _genericRepository.CountAsync();
        }

        public DataResponse<StudentThesisGroupOutput> Create(StudentThesisGroupInput input)
        {
            return _genericRepository.Create(input, GenerateUIDOptions.ShortUID);
        }

        public async Task<DataResponse<StudentThesisGroupOutput>> CreateAsync(StudentThesisGroupInput input)
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

        public StudentThesisGroupOutput Get(string id)
        {
            return _genericRepository.GetById(id);
        }

        public async Task<StudentThesisGroupOutput> GetAsync(string id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public List<StudentThesisGroupOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public Task<List<StudentThesisGroupOutput>> GetListAsync(int count = 200)
        {
            return _genericRepository.GetListAsync(count);
        }

        public Pagination<StudentThesisGroupOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<StudentThesisGroupOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }

        public DataResponse<StudentThesisGroupOutput> Update(string id, StudentThesisGroupInput input)
        {
            return _genericRepository.Update(id, input);
        }

        public async Task<DataResponse<StudentThesisGroupOutput>> UpdateAsync(string id, StudentThesisGroupInput input)
        {
            return await _genericRepository.UpdateAsync(id, input);
        }
    }
}
