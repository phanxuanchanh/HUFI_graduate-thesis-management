using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.DTO
{
    public class TopicInput
    {
        [Display(Name = "Mã chủ đề")]
        public string Id { get; set; }

        [Display(Name = "Tên chủ đề")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Mô tả chủ đề")]        
        public string Description { get; set; }

        [Display(Name = "Ghi chú chủ đề")]
        public string Notes { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [EmailAddress(ErrorMessage = "{0} không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Đia chỉ")]
        public string Adress { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Ngày xóa")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime DeletedAt { get; set; }
      
    }
    public class TopicOutput : TopicInput
    {
        public List<ThesisOutput> Theses { get; set; }
    }
}
