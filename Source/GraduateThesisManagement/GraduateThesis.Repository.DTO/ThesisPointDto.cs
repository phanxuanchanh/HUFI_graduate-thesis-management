
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GraduateThesis.Repository.DTO;

public class SupervisorPointInput
{
    [Display(Name = "Tên đề tài")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ThesisId { get; set; }

    [Display(Name = "Tên giảng viên")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string LecturerId { get; set; }

    [Display(Name = "Đánh giá về nội dung")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Contents { get; set; }

    [Display(Name = "Đánh giá về thái độ")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Attitudes { get; set; }

    [Display(Name = "Đánh giá về kết quả ")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Results { get; set; }

    [Display(Name = "Kết luận")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Conclusions { get; set; }

    [Display(Name = "Điểm")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public decimal? Point { get; set; }

    [Display(Name = "Ghi chú")]
    public string Notes { get; set; }
}

public class CLecturerPointInput
{
    [Display(Name = "Tên đề tài")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ThesisId { get; set; }

    [Display(Name = "Tên giảng viên")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string LecturerId { get; set; }

    [Display(Name = "Đánh giá về nội dung")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Contents { get; set; }

    [Display(Name = "Đánh giá về phương pháp nghiên cứu")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ResearchMethods { get; set; }

    [Display(Name = "Đánh giá về kết quả khoa học")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ScientificResults { get; set; }

    [Display(Name = "Đánh giá về Kết quả thực hành")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string PracticalResults { get; set; }

    [Display(Name = "Khuyết điểm của đề tài")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Defects { get; set; }

    [Display(Name = "Kết luận")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Conclusions { get; set; }

    [Display(Name = "Câu trả lời")] 
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Answers { get; set; }

    public decimal? Point { get; set; }
}
