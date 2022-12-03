using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO
{
    public class StudentInput
    {
        [Display(Name = "Mã sinh viên")]
        public string Id { get; set; }

        [Display(Name = "Tên sinh viên")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }


        [Display(Name = "Số điện thoại")]
        [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Độ dài điện thoại từ 10-11 kí tự")]
        [Phone(ErrorMessage = "{0} không hợp lệ")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [EmailAddress(ErrorMessage = "{0} không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Đia chỉ")]
        public string Adress { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Ảnh nhân viên")]
        public string Avatar { get; set; }

        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        [Display(Name = "Giới tính")]
        public string Sex { get; set; }
    }

    public class StudentOutput : StudentInput
    {
        public StudentClassOutput StudentClass { get; set; }

      
    }
}
