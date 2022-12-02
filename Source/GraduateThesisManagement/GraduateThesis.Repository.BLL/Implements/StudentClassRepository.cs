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
    public class StudentClassRepository : IStudentClassRepository
    {
        private HUFI_graduatethesisContext _context;
        private GenericRepository<HUFI_graduatethesisContext, StudentClass, StudentClassInput, StudentClassOutput> _genericRepository;

        internal StudentClassRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HUFI_graduatethesisContext, StudentClass, StudentClassInput, StudentClassOutput>(_context, _context.StudentClasses);

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
            _genericRepository.Selector = s => new StudentClassOutput
            {

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

        public DataResponse<StudentClassOutput> Create(StudentClassInput input)
        {
            return _genericRepository.Create(input, GenerateUIDOptions.ShortUID);
        }

        public async Task<DataResponse<StudentClassOutput>> CreateAsync(StudentClassInput input)
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

        public StudentClassOutput Get(string id)
        {
            return _genericRepository.GetById(id);
        }

        public async Task<StudentClassOutput> GetAsync(string id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public List<StudentClassOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public async Task<List<StudentClassOutput>> GetListAsync(int count = 200)
        {
            return await _genericRepository.GetListAsync();
        }

        public DataResponse<StudentClassOutput> Update(string id, StudentClassInput input)
        {
            return _genericRepository.Update(id, input);
        }

        public async Task<DataResponse<StudentClassOutput>> UpdateAsync(string id, StudentClassInput input)
        {
            return await _genericRepository.UpdateAsync(id, input);
        }
    }
}
