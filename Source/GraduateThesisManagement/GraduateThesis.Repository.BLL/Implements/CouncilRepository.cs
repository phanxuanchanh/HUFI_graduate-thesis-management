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
    public class CouncilRepository : ICouncilRepository
    {
        private HUFI_graduatethesisContext _context;
        private GenericRepository<HUFI_graduatethesisContext, Council, CouncilInput, CouncilOutput> _genericRepository;

        internal CouncilRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HUFI_graduatethesisContext, Council, CouncilInput, CouncilOutput>(context, context.Councils);

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
            
        }

        public void ConfigureSelectors()
        {
            _genericRepository.Selector = s => new CouncilOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Notes = s.Notes,
                Chairman = s.Chairman,
                //CouncilPoint = s.CouncilPoint,    


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

        public DataResponse<CouncilOutput> Create(CouncilInput input)
        {
            return _genericRepository.Create(input, GenerateUIDOptions.ShortUID);
        }

        public Task<DataResponse<CouncilOutput>> CreateAsync(CouncilInput input)
        {
            return _genericRepository.CreateAsync(input, GenerateUIDOptions.ShortUID);
        }

        public DataResponse ForceDelete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse> ForceDeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public CouncilOutput Get(string id)
        {
            return _genericRepository.GetById(id);
        }

        public Task<CouncilOutput> GetAsync(string id)
        {
            return _genericRepository.GetByIdAsync(id);
        }

        public List<CouncilOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public Task<List<CouncilOutput>> GetListAsync(int count = 200)
        {
            return _genericRepository.GetListAsync(count);
        }

        public Pagination<CouncilOutput> GetPagination(int page, int pageSize, string orderBy, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, keyword);
        }

        public async Task<Pagination<CouncilOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, keyword);
        }

        public DataResponse<CouncilOutput> Update(string id, CouncilInput input)
        {
            return _genericRepository.Update(id, input);
        }

        public async Task<DataResponse<CouncilOutput>> UpdateAsync(string id, CouncilInput input)
        {
            return await _genericRepository.UpdateAsync(id, input);
        }
    }
}
