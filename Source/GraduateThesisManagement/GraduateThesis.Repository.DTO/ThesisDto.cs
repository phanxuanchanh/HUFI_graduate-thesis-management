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

        [Display(Name = "Source Code đề tài")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string SourceCode { get; set; }
      
        [Display(Name = "Nhận xét đề tài")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string GeneralComment { get; set; }

        [Display(Name = "Năm làm đề tài")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int Year { get; set; }

        [Display(Name = "Ghi chú đề tài")]
        public string Notes { get; set; }

        public string TopicId { get; set; }
        public string CouncilId { get; set; }


        public bool IsDeleted { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Ngày chỉnh sửa")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Ngày xóa")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime DeletedAt { get; set; }
    
    }
    public class ThesisOutput : ThesisInput
    {
        public TopicOutput TopicClass { get; set; }
        public CouncilOutput CounciClass { get; set; }

    }

    
}
