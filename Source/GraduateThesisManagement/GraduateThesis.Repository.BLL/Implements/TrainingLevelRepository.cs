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
    public class TrainingLevelRepository : ITrainingLevelRepository
    {
        private HUFI_graduatethesisContext _context;
        private GenericRepository<HUFI_graduatethesisContext, TrainingLevel, TrainingLevelInput, TrainingLevelOutput> _genericRepository;

        internal TrainingLevelRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HUFI_graduatethesisContext, TrainingLevel, TrainingLevelInput, TrainingLevelOutput>(context, context.TrainingLevels);

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
            _genericRepository.IncludeMany(i => i.Theses);
        }

        public void ConfigureSelectors()
        {
            _genericRepository.PaginationSelector = s => new TrainingLevelOutput
            {
                Id = s.Id,
                Name = s.Name,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new TrainingLevelOutput
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
            return _genericRepository.Count();
        }

        public async Task<int> CountAsync()
        {
            return await _genericRepository.CountAsync();
        }

        public DataResponse<TrainingLevelOutput> Create(TrainingLevelInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<TrainingLevelOutput>> CreateAsync(TrainingLevelInput input)
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

        public TrainingLevelOutput Get(string id)
        {
            return _genericRepository.Get("Id", id);
        }

        public async Task<TrainingLevelOutput> GetAsync(string id)
        {
            return await _genericRepository.GetAsync("Id", id);
        }

        public List<TrainingLevelOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public Task<List<TrainingLevelOutput>> GetListAsync(int count = 200)
        {
            return _genericRepository.GetListAsync(count);
        }

        public Pagination<TrainingLevelOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<TrainingLevelOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }

        public DataResponse<TrainingLevelOutput> Update(string id, TrainingLevelInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<TrainingLevelOutput>> UpdateAsync(string id, TrainingLevelInput input)
        {
            throw new NotImplementedException();
        }
    }
}
