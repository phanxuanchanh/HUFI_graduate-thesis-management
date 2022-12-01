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
    public class ThesisRepository : IThesisRepository
    {
        private HUFI_graduatethesisContext _context;

        internal ThesisRepository(HUFI_graduatethesisContext context)
        {
            _context = context;

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
           
        }

        public void ConfigureSelectors()
        {
            
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public DataResponse<ThesisOutput> Create(ThesisInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<ThesisOutput>> CreateAsync(ThesisInput input)
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

        public ThesisOutput Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ThesisOutput> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public List<ThesisOutput> GetList(int count = 200)
        {
            throw new NotImplementedException();
        }

        public Task<List<ThesisOutput>> GetListAsync(int count = 200)
        {
            throw new NotImplementedException();
        }

        public DataResponse<ThesisOutput> Update(string id, ThesisInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<ThesisOutput>> UpdateAsync(string id, ThesisInput input)
        {
            throw new NotImplementedException();
        }
    }
}
