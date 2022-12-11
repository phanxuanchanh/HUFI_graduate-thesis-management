using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class ThesisCommittee
    {
        public ThesisCommittee()
        {
            CommitteeMembers = new HashSet<CommitteeMember>();
            ThesisCommitteeResults = new HashSet<ThesisCommitteeResult>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<CommitteeMember> CommitteeMembers { get; set; }
        public virtual ICollection<ThesisCommitteeResult> ThesisCommitteeResults { get; set; }
    }
}
