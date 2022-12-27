using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO
{
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

        [Display(Name = "File doc")]
        public string DocumentFile { get; set; }

        [Display(Name = "Tập Tin Trình Bày")]
        public string PresentationFile { get; set; }

        [Display(Name = "File PDF")]
        public string PdfFile { get; set; }

        [Display(Name = "SourceCode")]
        public string SourceCode { get; set; }

        public int Credits { get; set; }

        [Display(Name = "Năm thực hiện đề tài")]
        public string Year { get; set; }

        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        [Display(Name = "Chủ đề")]
        public string TopicId { get; set; }

        [Display(Name = "Khóa đào tạo")]
        public string TrainingFormId { get; set; }

        [Display(Name = "Cấp độ đào tạo")]
        public string TrainingLevelId { get; set; }

        [Display(Name = "Học kỳ")]
        public int Semester { get; set; }

        [Display(Name = "Nhóm thực hiện")]
        public string ThesisGroupId { get; set; }

        [Display(Name = "Chuyên ngành học")]
        public string SpecializationId { get; set; }

        [Display(Name = "Giảng viên ra đề tài")]
        public string LectureId { get; set; }

        [Display(Name = "Trạng thái duyệt")]
        public bool IsApproved { get; set; }
        
        public bool IsNew { get; set; }
        public bool InProgess { get; set; }
        public bool Finished { get; set; }
       
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
      

       
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