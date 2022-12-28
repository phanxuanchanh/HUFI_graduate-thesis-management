using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GraduateThesis.Repository.DTO
{
    public class FacultyStaffInput
    {
        [Display(Name = " Mã giảng viên khoa")]
        [Required(ErrorMessage = "{0} không được để trống ")]
        public string Id { get; set; }

        [Display(Name = "Mã khoa")]
        [Required(ErrorMessage = "{0} không được để trống ")]
        public string FacultyId { get; set; }


        [Display(Name = "Mã vai trò trong khoa")]
        [Required(ErrorMessage = "{0} không được để trống ")]
        public string FacultyRoleId { get; set; }

        [Display(Name = "Tên giảng viên")]
        [Required(ErrorMessage = "{0} không được để trống ")]
        public string FullName { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [StringLength(1, MinimumLength = 1, ErrorMessage = "Giới tính có 1 kí tự.")]
        [RegularExpression("Nam|Nữ", ErrorMessage = "Giới tính chỉ có 'Nam' hoặc 'Nữ'.")]
        [Required(ErrorMessage = "{0} Gioi tính Nam hoặc Nữ")]
        public string Gender { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Độ dài điện thoại từ 10-11 kí tự")]
        [Phone(ErrorMessage = "{0} không hợp lệ")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; }

        [Display(Name = "Chức vụ")]
        public string Position { get; set; }

        [Display(Name = "Bằng cấp")]
        public string Degree { get; set; }

        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "{0} bắt buộc nhập ")]
        [DataType(DataType.Password, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public string Password { get; set; }

        [DataType(DataType.Password, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
        public string Salt { get; set; }
    }

    public class FacultyStaffOutput : FacultyStaffInput
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public FacultyOutput Faculty { get; set; }
        public AppRolesInput FacultyStaffRole { get; set; }
    }

}
