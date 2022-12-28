using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.DTO
{
    public class AppRolesInput
    {
        [Display(Name = "Mã chức danh")]
        public string Id { get; set; }

        [Display(Name = "Tên vai trò , tên chức danh")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }

    public class AppRolesOutput : AppRolesInput
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
