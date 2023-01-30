using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class CommitteeMemberInput
{

    [Display(Name = "Mã thành viên hội đồng")]
    public string Id { get; set; }

    [Display(Name = " Tên tiểu ban")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ThesisCommitteeId { get; set; }

    [Display(Name = " Thành viên")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string MemberId { get; set; }

    [Display(Name = "Chức danh")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Titles { get; set; }
}
