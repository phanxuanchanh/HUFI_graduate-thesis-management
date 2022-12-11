using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class CommitteeMember
    {
        public CommitteeMember()
        {
            CommitteeMemberResults = new HashSet<CommitteeMemberResult>();
        }

        public string Id { get; set; }
        public string ThesisCommitteeId { get; set; }
        public string MemberId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Titles { get; set; }

        public virtual FacultyStaff Member { get; set; }
        public virtual ThesisCommittee ThesisCommittee { get; set; }
        public virtual ICollection<CommitteeMemberResult> CommitteeMemberResults { get; set; }
    }
}
