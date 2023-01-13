using System.Collections.Generic;

namespace GraduateThesis.Repository.DTO;

public class StudentThesisOutput
{
    public ThesisOutput Thesis { get; set; }
    public ThesisGroupOutput ThesisGroup { get; set; }
    public List<StudentOutput> Students { get; set; }
}
