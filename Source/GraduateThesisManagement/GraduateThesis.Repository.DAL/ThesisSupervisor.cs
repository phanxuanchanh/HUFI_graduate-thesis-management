using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class ThesisSupervisor
{
    public string ThesisId { get; set; }

    public string LectureId { get; set; }

    public string Contents { get; set; }

    public string Attitudes { get; set; }

    public string Results { get; set; }

    public string Conclusions { get; set; }

    public bool IsCompleted { get; set; }

    public int? Point { get; set; }

    public string Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual FacultyStaff Lecture { get; set; }

    public virtual Thesis Thesis { get; set; }
}
