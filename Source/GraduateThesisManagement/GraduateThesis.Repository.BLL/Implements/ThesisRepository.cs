using GraduateThesis.Common;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore.Storage;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements
{
    public class ThesisRepository : IThesisRepository
    {
        private HufiGraduateThesisContext _context;
        private GenericRepository<HufiGraduateThesisContext, Thesis, ThesisInput, ThesisOutput> _genericRepository;

        internal ThesisRepository(HufiGraduateThesisContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<HufiGraduateThesisContext, Thesis, ThesisInput, ThesisOutput>(context, context.Theses);

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

        public async Task<DataResponse> DoThesisRegisterAsync(ThesisRegisterInput thesisRegisterInput)
        {
            IDbContextTransaction dbContextTransaction = null;
            try
            {
                dbContextTransaction = _context.Database.BeginTransaction();

                Thesis thesis = await _context.Theses.FindAsync(thesisRegisterInput.ThesisId);
                if (thesis == null)
                    return new DataResponse { Status = DataResponseStatus.NotFound };

                string groupId = UID.GetShortUID();

                StudentThesisGroup studentThesisGroup = new StudentThesisGroup
                {
                    Id = groupId,
                    Name = thesisRegisterInput.GroupName,
                    Description = thesisRegisterInput.GroupDescription,
                    StudentQuantity = 0
                };

                await _context.StudentThesisGroups.AddAsync(studentThesisGroup);
                await _context.SaveChangesAsync();

                thesis.ThesisGroupId = groupId;
                await _context.SaveChangesAsync();

                List<StudentThesisGroupDetail> thesisGroupDetails = new List<StudentThesisGroupDetail>();
                string[] studentIdList = thesisRegisterInput.StudentIdList.Split(';');

                foreach (string studentId in studentIdList)
                {
                    thesisGroupDetails.Add(new StudentThesisGroupDetail
                    {
                        StudentThesisGroupId = groupId,
                        StudentId = studentId,
                        IsApproved = false
                    });
                }

                await _context.StudentThesisGroupDetails.AddRangeAsync(thesisGroupDetails);
                await _context.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();

                return new DataResponse { Status = DataResponseStatus.Success };
            }
            catch (Exception ex)
            {
                if (dbContextTransaction != null)
                    await dbContextTransaction.RollbackAsync();

                throw new Exception("The process was aborted because of an error!", ex);
            }
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

        public DataResponse ImportFromSpreadsheet(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            return _genericRepository.ImportFromSpreadsheet(stream, spreadsheetTypeOptions, sheetName, s =>
            {
                DateTime currentDateTime = DateTime.Now;
                Thesis thesis = new Thesis
                {
                    Id = UID.GetShortUID(),
                    CreatedAt = currentDateTime
                };

                thesis.Name = s.GetCell(1).StringCellValue;
                thesis.Description = s.GetCell(2).StringCellValue;

                return thesis;
            });
        }

        public async Task<DataResponse> ImportFromSpreadsheetAsync(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            return await _genericRepository.ImportFromSpreadsheetAsync(stream, spreadsheetTypeOptions, sheetName, s =>
            {
                DateTime currentDateTime = DateTime.Now;
                Thesis thesis = new Thesis
                {
                    Id = UID.GetShortUID(),
                    CreatedAt = currentDateTime
                };

                thesis.Name = s.GetCell(1).StringCellValue;
                thesis.Description = s.GetCell(2).StringCellValue;

                return thesis;
            });
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
