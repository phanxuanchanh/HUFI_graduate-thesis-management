using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.DTO
{
    public class CouncilMemberInput
    {

        [Display(Name = "Mã số lượng thành viên hội đồng")]
        public string Id { get; set; }

        [Display(Name = "Hội đồng")]
        public string councilId { get; set; }

    }
    public class CouncilMemberOutput : CouncilMemberInput
    {
        public CouncilOutput CounciClass { get; set; }

    }

  
}
