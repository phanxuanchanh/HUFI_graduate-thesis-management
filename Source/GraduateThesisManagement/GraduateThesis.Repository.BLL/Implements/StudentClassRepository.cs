﻿using GraduateThesis.Common;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements
{
    public class StudentClassRepository : IStudentClassRepository
    {
        private HufiGraduateThesisContext _context;
        private GenericRepository<HufiGraduateThesisContext, StudentClass, StudentClassInput, StudentClassOutput> _genericRepository;

        internal StudentClassRepository(HufiGraduateThesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HufiGraduateThesisContext, StudentClass, StudentClassInput, StudentClassOutput>(_context, _context.StudentClasses);

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
            _genericRepository.IncludeMany(i => i.Faculty);
        }

        public void ConfigureSelectors()
        {
            _genericRepository.PaginationSelector = s => new StudentClassOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                StudentQuantity= s.StudentQuantity,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt
            };

            _genericRepository.ListSelector = _genericRepository.PaginationSelector;
            _genericRepository.SingleSelector = s => new StudentClassOutput
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                StudentQuantity = s.StudentQuantity,
                Faculty = new FacultyOutput
                {
                    Id = s.Faculty.Id,
                    Name = s.Faculty.Name,
                    Description = s.Faculty.Description,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    DeletedAt = s.DeletedAt
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

        public IWorkbook ExportToSpreadsheet(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            return _genericRepository.ExportToSpreadsheet(spreadsheetTypeOptions, sheetName, includeProperties);
        }

        public async Task<IWorkbook> ExportToSpreadsheetAsync(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            return await _genericRepository
                .ExportToSpreadsheetAsync(spreadsheetTypeOptions, sheetName, includeProperties);
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
            return _genericRepository.Get("Id", id);
        }

        public async Task<StudentClassOutput> GetAsync(string id)
        {
            return await _genericRepository.GetAsync("Id", id);
        }

        public List<StudentClassOutput> GetList(int count = 200)
        {
            return _genericRepository.GetList(count);
        }

        public async Task<List<StudentClassOutput>> GetListAsync(int count = 200)
        {
            return await _genericRepository.GetListAsync();
        }

        public Pagination<StudentClassOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return _genericRepository.GetPagination(page, pageSize, orderBy, orderOptions, keyword);
        }

        public async Task<Pagination<StudentClassOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            return await _genericRepository.GetPaginationAsync(page, pageSize, orderBy, orderOptions, keyword);
        }

        public DataResponse ImportFromSpreadsheet(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            return _genericRepository.ImportFromSpreadsheet(stream, spreadsheetTypeOptions, sheetName, s =>
            {
                DateTime currentDateTime = DateTime.Now;
                StudentClass studentClass = new StudentClass
                {
                    Id = UID.GetShortUID(),
                    CreatedAt = currentDateTime
                };

                studentClass.Name = s.GetCell(1).StringCellValue;
                studentClass.Description = s.GetCell(2).StringCellValue;
                studentClass.StudentQuantity = (int)s.GetCell(3).NumericCellValue;
                studentClass.FacultyId = s.GetCell(4).StringCellValue;

                return studentClass;
            });
        }

        public async Task<DataResponse> ImportFromSpreadsheetAsync(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            return await _genericRepository.ImportFromSpreadsheetAsync(stream, spreadsheetTypeOptions, sheetName, s =>
            {
                DateTime currentDateTime = DateTime.Now;
                StudentClass studentClass = new StudentClass
                {
                    Id = UID.GetShortUID(),
                    CreatedAt = currentDateTime
                };

                studentClass.Name = s.GetCell(1).StringCellValue;
                studentClass.Description = s.GetCell(2).StringCellValue;
                studentClass.StudentQuantity = (int)s.GetCell(3).NumericCellValue;
                studentClass.FacultyId = s.GetCell(4).StringCellValue;

                return studentClass;
            });
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
