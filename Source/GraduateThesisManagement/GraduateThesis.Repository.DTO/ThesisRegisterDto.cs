
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO
{
    public class ThesisRegisterInput
    {
        public string ThesisId { get; set; }

        [Display(Name = "Tên nhóm đề tài")]
        [Required]
        public string GroupName { get; set; }

        [Display(Name = "Mô tả nhóm đề tài")]
        public string GroupDescription { get; set; }

        public string StudentIdList { get; set; }
    }
}
