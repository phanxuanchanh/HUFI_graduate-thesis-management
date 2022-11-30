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

        public CouncilMemberRepository(HUFI_graduatethesisContext context)
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

        public DataResponse<CouncilMemberOutput> Create(CouncilMemberInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<CouncilMemberOutput>> CreateAsync(CouncilMemberInput input)
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

        public CouncilMemberOutput Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<CouncilMemberOutput> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public List<CouncilMemberOutput> GetList(int count = 200)
        {
            throw new NotImplementedException();
        }

        public Task<List<CouncilMemberOutput>> GetListAsync(int count = 200)
        {
            throw new NotImplementedException();
        }

        public DataResponse<CouncilMemberOutput> Update(CouncilMemberInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<CouncilMemberOutput>> UpdateAsync(CouncilMemberInput input)
        {
            throw new NotImplementedException();
        }
    }
}
