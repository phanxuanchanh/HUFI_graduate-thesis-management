using GraduateThesis.Common;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements
{
    public class TrainingFormRepository : ITrainingFormRepository
    {
        private HUFI_graduatethesisContext _context;
        private GenericRepository<HUFI_graduatethesisContext, TrainingForm, TrainingFormInput, TrainingFormOutput> _genericRepository;

        internal TrainingFormRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HUFI_graduatethesisContext, TrainingForm, TrainingFormInput, TrainingFormOutput>(_context, _context.TrainingForms);

            ConfigureIncludes();
            ConfigureSelectors();
        }
        public DataResponse BatchDelete(string id)
        {
            return _genericRepository.BatchDelete(id);
        }

        public async Task<DataResponse> BatchDeleteAsync(string id)
        {
            return await _genericRepository.BatchDeleteAsync(id);
        }

        public void ConfigureIncludes()
        {
            _genericRepository.IncludeMany(i => i.Theses);
        }

        public void ConfigureSelectors()
        {
            _genericRepository.PaginationSelector = s => new TrainingFormOutput
            {
                Id = s.Id,
                Name = s.Name,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new TrainingFormOutput
            {
                Id = s.Id,
                Name = s.Name,
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

        public DataResponse<TrainingFormOutput> Create(TrainingFormInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<TrainingFormOutput>> CreateAsync(TrainingFormInput input)
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

        public TrainingFormOutput Get(string id)
        {
            return _genericRepository.Get("Id", id);
        }

        public async Task<TrainingFormOutput> GetAsync(string id)
        {
            return await _genericRepository.GetAsync("Id", id);
        }

        public List<TrainingFormOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public Task<List<TrainingFormOutput>> GetListAsync(int count = 200)
        {
            return _genericRepository.GetListAsync(count);
        }

        public Pagination<TrainingFormOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<TrainingFormOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }

        public DataResponse<TrainingFormOutput> Update(string id, TrainingFormInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<TrainingFormOutput>> UpdateAsync(string id, TrainingFormInput input)
        {
            throw new NotImplementedException();
        }
    }
}
