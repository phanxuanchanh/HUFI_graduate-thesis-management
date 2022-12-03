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
    public class CouncilMemberRepository : ICouncilMemberRepository
    {
        private HUFI_graduatethesisContext _context;
        private GenericRepository<HUFI_graduatethesisContext, CouncilMember, CouncilMemberInput, CouncilMemberOutput> _genericRepository;

        public CouncilMemberRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HUFI_graduatethesisContext, CouncilMember, CouncilMemberInput, CouncilMemberOutput>(context, context.CouncilMembers);

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
            
        }

        public void ConfigureSelectors()
        {
            _genericRepository.Selector = s => new CouncilMemberOutput
            {
                Id= s.Id,
                councilId= s.CouncilId,
                CounciClass = new CouncilOutput
                {
                   
                }
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

        public DataResponse<CouncilMemberOutput> Create(CouncilMemberInput input)
        {
            return _genericRepository.Create(input, GenerateUIDOptions.ShortUID);
        }

        public async Task<DataResponse<CouncilMemberOutput>> CreateAsync(CouncilMemberInput input)
        {
            return await _genericRepository.CreateAsync(input, GenerateUIDOptions.ShortUID);
        }

        public DataResponse ForceDelete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse> ForceDeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public CouncilMemberOutput Get(string id)
        {
            return _genericRepository.GetById(id);
        }

        public Task<CouncilMemberOutput> GetAsync(string id)
        {
            return _genericRepository.GetByIdAsync(id);
        }

        public List<CouncilMemberOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public async Task<List<CouncilMemberOutput>> GetListAsync(int count = 200)
        {
            return await _genericRepository.GetListAsync(count);
        }

        public Pagination<CouncilMemberOutput> GetPagination(int page, int pageSize, string orderBy, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, keyword);
        }

        public async Task<Pagination<CouncilMemberOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, keyword);
        }

        public DataResponse<CouncilMemberOutput> Update(string id, CouncilMemberInput input)
        {
            return _genericRepository.Update(id, input);
        }

        public Task<DataResponse<CouncilMemberOutput>> UpdateAsync(string id, CouncilMemberInput input)
        {
            return _genericRepository.UpdateAsync(id, input);
        }
    }
}
