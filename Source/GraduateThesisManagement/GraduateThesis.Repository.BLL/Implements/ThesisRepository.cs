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
    public class ThesisRepository : IThesisRepository
    {
        private HUFI_graduatethesisContext _context;
        private GenericRepository<HUFI_graduatethesisContext, Thesis, ThesisInput, ThesisOutput> _genericRepository;

        internal ThesisRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HUFI_graduatethesisContext, Thesis, ThesisInput, ThesisOutput>(context, context.Theses);

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
            _genericRepository.IncludeMany(i => i.Topic, i => i.ThesisGroup, i => i.ThesisGroup, i => i.TrainingForm, i => i.TrainingLevel, i => i.Specialization);
        }

        public void ConfigureSelectors()
        {
            _genericRepository.PaginationSelector = s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                MaxStudentNumber = s.MaxStudentNumber,
                ThesisGroupId = s.ThesisGroupId
            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new ThesisOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                MaxStudentNumber = s.MaxStudentNumber,
                SourceCode = s.SourceCode,
                Notes = s.Notes,
                TopicId = s.TopicId,
                TopicClass = new TopicOutput
                {
                    Name = s.Topic.Name,
                    Description = s.Topic.Description,
                    Id = s.Topic.Id,
                },
                StudentThesisGroup = (s.ThesisGroup == null ) ? null : new StudentThesisGroupOutput
                {
                    Id = s.ThesisGroup.Id,
                    Name = s.ThesisGroup!.Name,
                    Description = s.ThesisGroup!.Description,
                    StudentQuantity = s.ThesisGroup!.StudentQuantity
                },
                TrainingForm = (s.TrainingForm == null) ? null : new TrainingFormOutput
                {
                    Id = s.TrainingForm.Id,
                    Name = s.TrainingForm!.Name,                 
                },
                TrainingLevel =  new TrainingLevelOutput
                {
                    Id = s.TrainingLevel.Id,
                    Name = s.TrainingLevel!.Name,
                },
                Specialization = (s.Specialization == null) ? null : new SpecializationOutput
                {
                    Id = s.Specialization.Id,
                    Name = s.Specialization!.Name,
                },
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };
        }

        public int Count()
        {
            return _genericRepository.Count();
        }

        public Task<int> CountAsync()
        {
            return _genericRepository.CountAsync();
        }

        public DataResponse<ThesisOutput> Create(ThesisInput input)
        {
            return _genericRepository.Create(input, GenerateUIDOptions.ShortUID);
        }

        public async Task<DataResponse<ThesisOutput>> CreateAsync(ThesisInput input)
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

        public ThesisOutput Get(string id)
        {
            return _genericRepository.Get("Id", id);
        }

        public async Task<ThesisOutput> GetAsync(string id)
        {
            return await _genericRepository.GetAsync("Id", id);
        }

        public List<ThesisOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public async Task<List<ThesisOutput>> GetListAsync(int count = 200)
        {
            return await _genericRepository.GetListAsync(count);
        }

        public Pagination<ThesisOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<ThesisOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }

        public DataResponse<ThesisOutput> Update(string id, ThesisInput input)
        {
            return _genericRepository.Update(id, input);
        }

        public async Task<DataResponse<ThesisOutput>> UpdateAsync(string id, ThesisInput input)
        {
            return await _genericRepository.UpdateAsync(id, input);
        }
    }
}
