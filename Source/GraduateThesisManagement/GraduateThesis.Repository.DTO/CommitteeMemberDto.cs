using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class CommitteeMemberInput
{

    [Display(Name = "Mã thành viên hội đồng")]
    [Required(ErrorMessage = "{0} không được để trống ")]
    public string Id { get; set; }

    [Display(Name = " Mã Hội đồng")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string councilId { get; set; }
    
}
public class CommitteeMemberOutput : CommitteeMemberInput
{
    public CommitteeMemberOutput CommitteeMember { get; set; }

}