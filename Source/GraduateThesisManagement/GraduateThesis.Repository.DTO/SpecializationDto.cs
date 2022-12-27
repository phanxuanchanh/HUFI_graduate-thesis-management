using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraduateThesis.Repository.DTO
{
    public class SpecializationInput
    {
        [Display(Name = "Mã chuyên ngành")]
        [Required(ErrorMessage = "{0} không được để trống ")]
        public string Id { get; set; }

        [Display(Name = "Tên chuyên ngành")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }

    public class SpecializationOutput : SpecializationInput
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


    }
}
