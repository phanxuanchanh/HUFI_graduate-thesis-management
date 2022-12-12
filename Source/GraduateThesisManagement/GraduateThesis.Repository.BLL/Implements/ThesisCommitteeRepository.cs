﻿using GraduateThesis.Generics;
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
    public class ThesisCommitteeRepository : IThesisCommitteeRepository
    {
        private HUFI_graduatethesisContext _context;
        private GenericRepository<HUFI_graduatethesisContext, ThesisCommittee, ThesisCommitteeInput, ThesisCommitteeOutput> _genericRepository;

        internal ThesisCommitteeRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HUFI_graduatethesisContext, ThesisCommittee, ThesisCommitteeInput, ThesisCommitteeOutput>(context, context.ThesisCommittees);

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
            _genericRepository.PaginationSelector = s => new ThesisCommitteeOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,  
            };

            _genericRepository.ListSelector = _genericRepository.ListSelector;
            _genericRepository.SingleSelector = s => new ThesisCommitteeOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
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

        public DataResponse<ThesisCommitteeOutput> Create(ThesisCommitteeInput input)
        {
            return _genericRepository.Create(input, GenerateUIDOptions.ShortUID);
        }

        public Task<DataResponse<ThesisCommitteeOutput>> CreateAsync(ThesisCommitteeInput input)
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

        public ThesisCommitteeOutput Get(string id)
        {
            return _genericRepository.GetById(id);
        }

        public Task<ThesisCommitteeOutput> GetAsync(string id)
        {
            return _genericRepository.GetByIdAsync(id);
        }

        public List<ThesisCommitteeOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public Task<List<ThesisCommitteeOutput>> GetListAsync(int count = 200)
        {
            return _genericRepository.GetListAsync(count);
        }

        public Pagination<ThesisCommitteeOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<ThesisCommitteeOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }

        public DataResponse<ThesisCommitteeOutput> Update(string id, ThesisCommitteeInput input)
        {
            return _genericRepository.Update(id, input);
        }

        public async Task<DataResponse<ThesisCommitteeOutput>> UpdateAsync(string id, ThesisCommitteeInput input)
        {
            return await _genericRepository.UpdateAsync(id, input);
        }
    }
}
