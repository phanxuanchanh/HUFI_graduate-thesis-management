using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GraduateThesis.Repository.DTO
{
    public class FacultyInput
    {
        [Display(Name = "Mã khoa")]
        [Required(ErrorMessage = "{0} không được để trống ")]
        public string Id { get; set; }

        [Display(Name = " Tên khoa ")]
        [Required(ErrorMessage = "{0} không được để trống ")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }

    public class FacultyOutput : FacultyInput
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<FacultyStaffOutput> FacultyStaffs { get; set; }
        public List<StudentClassOutput> StudentClasses { get; set; }

        public AppRolesOutput FacultyStaffRole { get; set; }
    }
}
