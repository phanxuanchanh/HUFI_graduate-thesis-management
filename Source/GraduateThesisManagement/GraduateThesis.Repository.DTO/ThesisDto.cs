using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.DTO
{
    public class ThesisInput
    {
        [Display(Name = "Mã đề tài")]
        public string Id { get; set; }

        [Display(Name = "Tên đề tài")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Mô tả đề tài")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Description { get; set; }

        [Display(Name = "Số lượng sinh viên")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int MaxStudentNumber { get; set; }

        public string DocumentFile { get; set; }

        public string PresentationFile { get; set; }

        public string PdfFile { get; set; }

        public string SourceCode { get; set; }

        public int  Credits { get; set; }

        [Display(Name = "Năm làm đề tài")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int Year { get; set; }

        [Display(Name = "Ghi chú đề tài")]
        public string Notes { get; set; }

        public string TopicId { get; set; }

        public string CouncilId { get; set; }

        public string TrainingFormId { get; set; }

        public string TrainingLevelId { get; set; }

        public bool IsApproved { get; set; }

        public bool IsNew { get; set; }

        public bool InProgess { get; set; }

        public bool Finished { get; set; }

        public string SpecializationId { get; set; }

        public DateTime DateFrom { get; set; }
        
        public DateTime DateTo { get; set; }

        public string LectureId { get; set; }
    
    }
    public class ThesisOutput : ThesisInput
    {
        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Ngày chỉnh sửa")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Ngày xóa")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime DeletedAt { get; set; }

        public TopicOutput TopicClass { get; set; }
    }

    
}
