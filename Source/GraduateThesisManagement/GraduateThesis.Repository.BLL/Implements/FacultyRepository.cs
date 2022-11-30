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
    public class FacultyRepository : IFacultyRepository
    {
        private HUFI_graduatethesisContext context;

        public FacultyRepository(HUFI_graduatethesisContext context)
        {
            this.context = context;
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

        public DataResponse<FacultyOutput> Create(FacultyInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<FacultyOutput>> CreateAsync(FacultyInput input)
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

        public FacultyOutput Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<FacultyOutput> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public List<FacultyOutput> GetList(int count = 200)
        {
            throw new NotImplementedException();
        }

        public Task<List<FacultyOutput>> GetListAsync(int count = 200)
        {
            throw new NotImplementedException();
        }

        public DataResponse<FacultyOutput> Update(FacultyInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<FacultyOutput>> UpdateAsync(FacultyInput input)
        {
            throw new NotImplementedException();
        }
    }
}
