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
    public class StudentRepository : IStudentRepository
    {
        private HUFI_graduatethesisContext _context;

        internal StudentRepository(HUFI_graduatethesisContext context)
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

        public DataResponse<StudentOutput> Create(StudentInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<StudentOutput>> CreateAsync(StudentInput input)
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

        public StudentOutput Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<StudentOutput> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public List<StudentOutput> GetList(int count = 200)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentOutput>> GetListAsync(int count = 200)
        {
            throw new NotImplementedException();
        }

        public DataResponse<StudentOutput> Update(StudentInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<StudentOutput>> UpdateAsync(StudentInput input)
        {
            throw new NotImplementedException();
        }
    }
}
