using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO
{
    public class ThesisInput
    {
        [Display(Name = "Mã khóa luận")]
        public string Id { get; set; }

        [Display(Name = "Tên khóa luận")]
        public string Name { get; set; }

        [Display(Name = "Các mô tả, yêu cầu")]
        public string Description { get; set; }

        [Display(Name = "Số sinh viên tối đa")]
        public int MaxStudentNumber { get; set; }

        [Display(Name = "File tài liệu")]
        public string DocumentFile { get; set; }

        [Display(Name = "Tập tin trình bày")]
        public string PresentationFile { get; set; }

        [Display(Name = "File PDF")]
        public string PdfFile { get; set; }

        [Display(Name = "SourceCode đồ án")]
        public string SourceCode { get; set; }

        [Display(Name = "Số tín chỉ")]
        public int Credits { get; set; }

        [Display(Name = "Năm thực hiện đồ án")]
        public string Year { get; set; }

        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        [Display(Name = "Chủ đề")]
        public string TopicId { get; set; }

        [Display(Name = "Mã hình thức đào tạo")]
        public string TrainingFormId { get; set; }

        [Display(Name = "mã cấp độ đào tạo")]
        public string TrainingLevelId { get; set; }

        [Display(Name = "Xét duyệt")]
        public bool IsApproved { get; set; }

        [Display(Name = "Mới")]
        public bool IsNew { get; set; }

        [Display(Name = "Đang tiến hành")]
        public bool InProgess { get; set; }

        [Display(Name = "Đã hoàn thành")]
        public bool Finished { get; set; }

        [Display(Name = "Mã chuyên ngành")]
        public string SpecializationId { get; set; }

        [Display(Name = "Từ ngày")]
        [Range(typeof(DateTime), "1/2/2004", "3/4/2004",
        ErrorMessage = "chọn 1 hoặc 2")]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Đến ngày")]
        [Range(typeof(DateTime), "1/2/2004", "3/4/2004",
        ErrorMessage = "chọn 1 hoặc 2")]
        public DateTime DateTo { get; set; }

        public string LectureId { get; set; }

        [Display(Name = "Học kỳ")]
        public int Semester { get; set; }

        [Display(Name = "Mã nhóm luận văn")]
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
}