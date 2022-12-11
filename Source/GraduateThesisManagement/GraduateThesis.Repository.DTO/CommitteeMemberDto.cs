using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.DTO
{
    public class CommitteeMemberInput
    {

        [Display(Name = "Mã số lượng thành viên hội đồng")]
        public string Id { get; set; }

        [Display(Name = "Hội đồng")]
        public string councilId { get; set; }

    }
    public class CommitteeMemberOutput : CommitteeMemberInput
    {
        public CommitteeMemberOutput CommitteeMember { get; set; }

    }

  
}
