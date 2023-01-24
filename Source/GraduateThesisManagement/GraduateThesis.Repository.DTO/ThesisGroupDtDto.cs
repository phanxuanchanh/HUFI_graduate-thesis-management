using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DTO;

public class ThesisGroupDtOutput
{
    public string ThesisId { get; set; }
    public string ThesisName { get; set; }
    public string GroupId { get; set; }
    public string GroupName { get; set; }
    public string GroupDescription { get; set; }
    public int StudentQuantity { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public string GroupNotes { get; set; }
    public string MemberNotes { get; set; }
    public bool Group_IsCompleted { get; set; }
    public bool Group_IsFinished { get; set; }
    public bool Member_IsApproved { get; set; }
    public bool Member_IsCompleted { get; set; }
    public bool Member_IsFinished { get; set; }

    public List<StudentOutput> Students { get; set; }
}
