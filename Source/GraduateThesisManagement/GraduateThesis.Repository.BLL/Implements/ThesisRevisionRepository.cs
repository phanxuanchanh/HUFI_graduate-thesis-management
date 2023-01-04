using GraduateThesis.ApplicationCore.Repository;
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

    public class ThesisRevisionRepository : SubRepository<ThesisRevision, ThesisRevisionInput, ThesisRevisionOutput, string>, IThesisRevisionRepository
    {
        private HufiGraduateThesisContext _context;

        internal ThesisRevisionRepository(HufiGraduateThesisContext context)
            : base(context, context.ThesisRevisions)
        {
            _context = context;
        }
        protected override void ConfigureIncludes()
        {
            _genericRepository.IncludeMany(i => i.Thesis);
        }

        protected override void ConfigureSelectors()
        {
            PaginationSelector = s => new ThesisRevisionOutput
            {
                Id = s.Id,
                Title = s.Title,
                Summary = s.Summary,
                DocumentFile = s.DocumentFile,
                PresentationFile = s.PresentationFile,
                PdfFile = s.PdfFile,
                SourceCode = s.SourceCode,
               
            };

            ListSelector = PaginationSelector;
            SingleSelector = s => new ThesisRevisionOutput
            {
                Id = s.Id,
                Title = s.Title,
                Summary = s.Summary,
                DocumentFile = s.DocumentFile,
                PresentationFile = s.PresentationFile,
                PdfFile = s.PdfFile,
                SourceCode = s.SourceCode,
                thesis = new ThesisOutput
                {
                    Id = s.Thesis.Id,
                    Name = s.Thesis.Name,
                    Description = s.Thesis.Description,
                    DocumentFile = s.Thesis.DocumentFile,
                    PdfFile = s.Thesis.PdfFile,
                    PresentationFile = s.Thesis.PresentationFile,
                    SourceCode = s.Thesis.SourceCode,
                    MaxStudentNumber = s.Thesis.MaxStudentNumber,
                    Credits = s.Thesis.Credits,
                    Year = s.Thesis.Year,
                    Notes = s.Thesis.Notes,
                    TopicId = s.Thesis.TopicId,
                    TrainingFormId = s.Thesis.TrainingFormId,
                    TrainingLevelId = s.Thesis.TrainingLevelId,
                    IsApproved = s.Thesis.IsApproved,
                    IsNew = s.Thesis.IsNew,
                    InProgess = s.Thesis.InProgess,
                    Finished = s.Thesis.Finished,
                    SpecializationId = s.Thesis.SpecializationId,
                    DateFrom = s.Thesis.DateFrom,
                    DateTo = s.Thesis.DateTo,
                    LectureId = s.Thesis.LectureId,
                    Semester = s.Thesis.Semester,
                    ThesisGroupId = s.Thesis.ThesisGroupId,
                    CreatedAt = s.Thesis.CreatedAt,
                    UpdatedAt = s.Thesis.UpdatedAt,
                    DeletedAt = s.Thesis.DeletedAt

                },
                CreatedAt = (DateTime)s.Thesis.CreatedAt,
                UpdatedAt = (DateTime)s.Thesis.UpdatedAt,
                DeletedAt = (DateTime)s.Thesis.DeletedAt
            };
        }
    }
}
