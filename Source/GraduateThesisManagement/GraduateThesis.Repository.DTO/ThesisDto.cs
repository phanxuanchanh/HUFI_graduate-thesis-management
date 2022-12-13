﻿using System;

namespace GraduateThesis.Repository.DTO
{
    public class ThesisInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxStudentNumber { get; set; }
        public string DocumentFile { get; set; }
        public string PresentationFile { get; set; }
        public string PdfFile { get; set; }
        public string SourceCode { get; set; }
        public int Credits { get; set; }
        public string Year { get; set; }
        public string Notes { get; set; }
        public string TopicId { get; set; }
        public string TrainingFormId { get; set; }
        public string TrainingLevelId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsNew { get; set; }
        public bool InProgess { get; set; }
        public bool Finished { get; set; }
        public string SpecializationId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string LectureId { get; set; }
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
}