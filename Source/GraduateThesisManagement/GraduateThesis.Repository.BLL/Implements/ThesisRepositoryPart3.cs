using GraduateThesis.ExtensionMethods;
using GraduateThesis.Repository.BLL.Consts;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GraduateThesis.Repository.BLL.Implements;

public partial class ThesisRepository
{
    public async Task<byte[]> ExportAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses
                .Include(i => i.Topic).Include(i => i.Specialization).Include(i => i.Lecture)
                .Where(t => t.IsDeleted == false && t.Lecture.IsDeleted == false)
                .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Name = "DANH SÁCH ĐỀ TÀI KHÓA LUẬN - KHOA CNTT",
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name,
                    LectureName = $"{s.Lecture.Surname.Trim(' ')} {s.Lecture.Name.Trim(' ')}"
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportPndgThesesAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "pending-thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses
                .Include(i => i.Topic).Include(i => i.Specialization).Include(i => i.Lecture)
                .Where(t => t.StatusId == ThesisStatusConsts.Pending && t.IsDeleted == false && t.Lecture.IsDeleted == false)
                .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Name = "DANH SÁCH ĐỀ TÀI ĐANG CHỜ DUYỆT - KHOA CNTT",
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name,
                    LectureName = $"{s.Lecture.Surname.Trim(' ')} {s.Lecture.Name.Trim(' ')}"
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportPubldThesesAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "published-thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses
                .Include(i => i.Topic).Include(i => i.Specialization).Include(i => i.Lecture)
                .Where(t => t.StatusId == ThesisStatusConsts.Published && t.IsDeleted == false)
                .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name,
                    LectureName = $"{s.Lecture.Surname.Trim(' ')} {s.Lecture.Name.Trim(' ')}"
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportRejdThesesAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "rejected-thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses
                .Include(i => i.Topic).Include(i => i.Specialization).Include(i => i.Lecture)
                .Where(t => t.StatusId == ThesisStatusConsts.Rejected && t.IsDeleted == false && t.Lecture.IsDeleted == false)
                .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Name = "DANH SÁCH ĐỀ TÀI ĐÃ BỊ TỪ CHỐI DUYỆT - KHOA CNTT",
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name,
                    LectureName = $"{s.Lecture.Surname.Trim(' ')} {s.Lecture.Name.Trim(' ')}",
                    Notes = s.Notes
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportAppdThesesAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses.Include(i => i.Topic).Include(i => i.Specialization)
            .Where(t => t.StatusId == ThesisStatusConsts.Approved && t.IsDeleted == false)
            .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportAsync(string lecturerId)
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses.Include(i => i.Topic).Include(i => i.Specialization)
            .Where(t => t.LectureId == lecturerId && t.IsDeleted == false)
            .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportPubldThesesAsync(string lecturerId)
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses.Include(i => i.Topic).Include(i => i.Specialization)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Published && t.IsDeleted == false)
            .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportRejdThesesAsync(string lecturerId)
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses.Include(i => i.Topic).Include(i => i.Specialization)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Rejected && t.IsDeleted == false)
            .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<byte[]> ExportAppdThesesAsync(string lecturerId)
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "thesis_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Thesis> theses = await _context.Theses.Include(i => i.Topic).Include(i => i.Specialization)
            .Where(t => t.LectureId == lecturerId && t.StatusId == ThesisStatusConsts.Approved && t.IsDeleted == false)
            .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Items = theses.Select(s => new ThesisExport
                {
                    Index = count++,
                    Id = s.Id,
                    Name = s.Name,
                    Description = HttpUtility.HtmlDecode(s.Description).RemoveHtmlTag(),
                    Credits = s.Credits,
                    MaxStudentNumber = s.MaxStudentNumber,
                    Year = s.Year,
                    Semester = s.Semester,
                    TopicName = s.Topic.Name,
                    SpecializationName = s.Specialization.Name
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }
}
