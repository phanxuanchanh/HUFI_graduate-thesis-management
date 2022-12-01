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
    public class StudentThesisGroupRepository : IStudentThesisGroupRepository
    {
        private HUFI_graduatethesisContext _context;
        private GenericRepository<HUFI_graduatethesisContext, StudentThesisGroup, StudentThesisGroupInput, StudentThesisGroupOutput> _genericRepository;

        public StudentThesisGroupRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HUFI_graduatethesisContext, StudentThesisGroup, StudentThesisGroupInput, StudentThesisGroupOutput>(context, context.StudentThesisGroups);

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
