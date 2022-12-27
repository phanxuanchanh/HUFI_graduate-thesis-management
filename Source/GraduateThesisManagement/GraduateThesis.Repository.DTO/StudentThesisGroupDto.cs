using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraduateThesis.Repository.DTO
{
    public class StudentThesisGroupInput
    {
        [Display(Name = "Mã nhóm sinh viên")]
        public string Id { get; set; }

        [Display(Name = "Tên nhóm")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Số lượng sinh viên")]
        public int StudentQuantity { get; set; }

        [Display(Name = "Mô tả")]
        public string Notes { get; set; }
    }

    public class StudentThesisGroupOutput : StudentThesisGroupInput
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ThesisOutput Thesis { get; set; }
    }

}
