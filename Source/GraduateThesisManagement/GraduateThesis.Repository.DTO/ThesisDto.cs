using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class ThesisInput
{
    [Display(Name = "Mã đề tài")]
    public string Id { get; set; }

    [Display(Name = "Tên đề tài")]
    public string Name { get; set; }

    [Display(Name = "Các mô tả, yêu cầu")]
    public string Description { get; set; }

    [Display(Name = "Số sinh viên tối đa")]
    public int MaxStudentNumber { get; set; }

    [Display(Name = "File tài liệu")]
    public string DocumentFile { get; set; }
    public string PresentationFile { get; set; }

    [Display(Name = "File PDF")]
    public string PdfFile { get; set; }
    public string SourceCode { get; set; }
    public int Credits { get; set; }
    public string Year { get; set; }

    [Display(Name = "Ghi chú")]
    public string Notes { get; set; }

    [Display(Name = "Chủ đề")]
    public string TopicId { get; set; }
    public string TrainingFormId { get; set; }
    public string TrainingLevelId { get; set; }
    public bool IsApproved { get; set; }
    public bool IsNew { get; set; }

    [Display(Name = "Đang tiến hành")]
    public bool InProgess { get; set; }

    [Display(Name = "Đã hoàn thành")]
    public bool Finished { get; set; }
    public string SpecializationId { get; set; }

    [Display(Name = "Từ ngày")]
    public DateTime DateFrom { get; set; }

    [Display(Name = "Đến ngày")]
    public DateTime DateTo { get; set; }
    public string LectureId { get; set; }

    [Display(Name = "Học kỳ")]
    public int Semester { get; set; }
    public string ThesisGroupId { get; set; }
}

public class ThesisOutput : ThesisInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public TopicOutput TopicClass { get; set; }

    public StudentThesisGroupOutput StudentThesisGroup { get; set; }

    public TrainingLevelOutput TrainingLevel { get; set; }

    public TrainingFormOutput TrainingForm { get; set; }

    public FacultyStaffOutput FacultyStaf { get; set; }

    public SpecializationOutput Specialization { get; set; }
}