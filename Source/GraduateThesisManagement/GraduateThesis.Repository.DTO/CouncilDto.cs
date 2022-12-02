using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.DTO
{
    public class CouncilInput
    {
        [Display(Name = "Mã hội đồng")]
        public string Id { get; set; }

        [Display(Name = "Tên hội đồng")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Mô tả hội đồng")]
        public string Description { get; set; }

        [Display(Name = "Ghi chú hội đồng")]
        public string Notes { get; set; }


        [Display(Name = "Chủ tịch hội đồng")]
        public string Chairman { get; set; }


        [Display(Name = "Điểm hội đồng")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(maximumLength: 2, MinimumLength = 0, ErrorMessage = "Điểm có độ dài từ 0 đến 2 ký tự")]
        public string CouncilPoint { get; set; }

        [Display(Name = "Ngày xét duyệt")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Ngày xóa")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime DeletedAt { get; set; }

    }
    public class CounciOutput : CouncilInput
    {
    }
}
