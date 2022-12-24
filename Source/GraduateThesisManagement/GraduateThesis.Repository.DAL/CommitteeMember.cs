using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class CommitteeMember
{
    public string Id { get; set; }

    public string ThesisCommitteeId { get; set; }

    public string MemberId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public string Titles { get; set; }

    public virtual ICollection<CommitteeMemberResult> CommitteeMemberResults { get; } = new List<CommitteeMemberResult>();

    public virtual FacultyStaff Member { get; set; }

    public virtual ThesisCommittee ThesisCommittee { get; set; }
}
