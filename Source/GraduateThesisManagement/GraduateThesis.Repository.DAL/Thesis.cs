using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Thesis
    {
        public Thesis()
        {
            CommitteeMemberResults = new HashSet<CommitteeMemberResult>();
            ImplementationPlans = new HashSet<ImplementationPlan>();
            StudentThesisGroups = new HashSet<StudentThesisGroup>();
            ThesisRevisions = new HashSet<ThesisRevision>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxStudentNumber { get; set; }
        public string DocumentFile { get; set; }
        public string PresentationFile { get; set; }
        public string PdfFile { get; set; }
        public string SourceCode { get; set; }
        public int Credits { get; set; }
        public int Year { get; set; }
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
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string LectureId { get; set; }

        public virtual FacultyStaff Lecture { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual TrainingForm TrainingForm { get; set; }
        public virtual TrainingLevel TrainingLevel { get; set; }
        public virtual CounterArgumentResult CounterArgumentResult { get; set; }
        public virtual ThesisCommitteeResult ThesisCommitteeResult { get; set; }
        public virtual ThesisSupervisor ThesisSupervisor { get; set; }
        public virtual ICollection<CommitteeMemberResult> CommitteeMemberResults { get; set; }
        public virtual ICollection<ImplementationPlan> ImplementationPlans { get; set; }
        public virtual ICollection<StudentThesisGroup> StudentThesisGroups { get; set; }
        public virtual ICollection<ThesisRevision> ThesisRevisions { get; set; }
    }
}
