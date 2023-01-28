using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraduateThesis.Repository.DTO
{
    public class CouncilInput
    {
        [Display(Name = "Mã hội đồng")]
        public string Id { get; set; }

        [Display(Name = "Tên hội đồng")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Học kỳ")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public int Semester { get; set; }

        [Display(Name = "Năm học")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Year { get; set; }
    }

    public class CouncilOutput : CouncilInput
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
