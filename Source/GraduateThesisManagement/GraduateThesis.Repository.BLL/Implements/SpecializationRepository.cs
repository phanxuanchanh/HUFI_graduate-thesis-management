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
    public class SpecializationRepository : ISpecializationRepository
    {
        private HUFI_graduatethesisContext _context;
        private GenericRepository<HUFI_graduatethesisContext, Specialization, SpecializationInput, SpecializationOutput> _genericRepository;

        internal SpecializationRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HUFI_graduatethesisContext, Specialization, SpecializationInput, SpecializationOutput>(context, context.Specializations);

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
            _genericRepository.IncludeMany(i => i.Theses);
        }

        public void ConfigureSelectors()
        {
            _genericRepository.PaginationSelector = s => new SpecializationOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new SpecializationOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public DataResponse<SpecializationOutput> Create(SpecializationInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<SpecializationOutput>> CreateAsync(SpecializationInput input)
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
        public SpecializationOutput Get(string id)
        {
            return _genericRepository.Get("Id", id);
        }

        public async Task<SpecializationOutput> GetAsync(string id)
        {
            return await _genericRepository.GetAsync("Id", id);
        }

        public List<SpecializationOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public Task<List<SpecializationOutput>> GetListAsync(int count = 200)
        {
            return _genericRepository.GetListAsync(count);
        }

        public Pagination<SpecializationOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<SpecializationOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }
        public DataResponse<SpecializationOutput> Update(string id, SpecializationInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<SpecializationOutput>> UpdateAsync(string id, SpecializationInput input)
        {
            throw new NotImplementedException();
        }
    }
}
