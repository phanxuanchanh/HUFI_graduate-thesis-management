using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.DTO
{
    public class LecturerInput
    {
        [Display(Name = "Mã giảng viên")]
        public string Id { get; set; }

        [Display(Name = "Tên giảng viên")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Mô tả giảng viên")]
        public string Description { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Độ dài điện thoại từ 10-11 kí tự")]
        [Phone(ErrorMessage = "{0} không hợp lệ")]
        public string Phone { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Độ dài điện thoại từ 10-11 kí tự")]
        [Phone(ErrorMessage = "{0} không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Adress { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Avatar { get; set; }

        [Display(Name = "Giới tính")]
        public string Sex { get; set; }

        [Display(Name = "Chức vụ giảng viên")]
        public string Position { get; set; }

        [Display(Name = "Trình độ giảng viên")]
        public string Degree { get; set; }

        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }


    }
    public class LecturerOutput : LecturerInput
    {
        
    }

}
