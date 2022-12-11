using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class CounterArgumentResult
    {
        public string ThesisId { get; set; }
        public string LectureId { get; set; }
        public string Contents { get; set; }
        public string ResearchMethods { get; set; }
        public string ScientificResults { get; set; }
        public string PracticalResults { get; set; }
        public string Defects { get; set; }
        public string Conclusions { get; set; }
        public string Questions { get; set; }
        public decimal? Point { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual FacultyStaff Lecture { get; set; }
        public virtual Thesis Thesis { get; set; }
    }
}
