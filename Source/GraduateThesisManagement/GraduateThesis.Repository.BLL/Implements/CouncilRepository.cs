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

        internal CouncilRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
        }

        public DataResponse BatchDelete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse> BatchDeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public DataResponse<CouncilOutput> Create(CouncilInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<CouncilOutput>> CreateAsync(CouncilInput input)
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

        public CouncilOutput Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<CouncilOutput> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public List<CouncilOutput> GetList(int count = 200)
        {
            throw new NotImplementedException();
        }

        public Task<List<CouncilOutput>> GetListAsync(int count = 200)
        {
            throw new NotImplementedException();
        }

        public DataResponse<CouncilOutput> Update(CouncilInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<CouncilOutput>> UpdateAsync(CouncilInput input)
        {
            throw new NotImplementedException();
        }
    }
}
