using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class ThesisGroupDetail
{
    public string StudentThesisGroupId { get; set; }

    public string StudentId { get; set; }

    public string Notes { get; set; }

    public bool IsLeader { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsFinished { get; set; }

    public bool IsApproved { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Student Student { get; set; }

    public virtual ThesisGroup StudentThesisGroup { get; set; }
}
