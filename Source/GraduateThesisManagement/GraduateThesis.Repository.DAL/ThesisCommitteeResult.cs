using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class ThesisCommitteeResult
    {
        public string ThesisId { get; set; }
        public string ThesisCommitteeId { get; set; }
        public string Contents { get; set; }
        public string Conclusions { get; set; }
        public decimal? Point { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Thesis Thesis { get; set; }
        public virtual ThesisCommittee ThesisCommittee { get; set; }
    }
}
