using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraduateThesis.Repository.DTO
{
    public class StudentClassInput
    {
        [Display(Name = "Mã lớp học")]
        [Required(ErrorMessage = "{0} Bắt buộc nhập ")]
        public string Id { get; set; }

        [Display(Name = "Tên chuyên ngành")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Số lượng sinh viên của lớp")]
        public int StudentQuantity { get; set; }

        [Display(Name = "Mã khoa")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string FacultyId { get; set; }
    }

    public class StudentClassOutput : StudentClassInput
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public FacultyOutput Faculty { get; set; }
        public List<StudentOutput> Students { get; set; }
    }
}
