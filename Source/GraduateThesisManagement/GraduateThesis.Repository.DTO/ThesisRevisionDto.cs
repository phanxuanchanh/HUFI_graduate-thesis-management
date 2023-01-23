using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class ThesisRevisionInput
{
    [Display(Name = "Mã đề tài chỉnh sửa")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Id { get; set; }

    [Display(Name = "Mã đề tài ")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string ThesisId { get; set; }

    [Display(Name = "Tóm tắt")]
    public string Summary { get; set; }

    [Display(Name = "Tiêu đề")]
    public string Title { get; set; }

    [Display(Name = "File Doc")]
    public string DocumentFile { get; set; }

    [Display(Name = "File trình bày")]
    public string PresentationFile { get; set; }

    [Display(Name = "File PDF")]
    public string PdfFile { get; set; }

    [Display(Name = "Source code")]
    public string SourceCode { get; set; }
}

public class ThesisRevisionOutput : ThesisRevisionInput
{
    [Display(Name = "Ngày xét duyệt")]
    [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Ngày cập nhật")]
    [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
    public DateTime? UpdatedAt { get; set; }

    [Display(Name = "Ngày xóa")]
    [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
    public DateTime? DeletedAt { get; set; }


    public ThesisOutput Thesis { get; set; }
}
