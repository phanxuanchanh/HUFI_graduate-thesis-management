using System.Collections.Generic;

namespace GraduateThesis.Repository.DTO
{
    public class StudentThesisOutput
    {
        public ThesisOutput Thesis { get; set; }
        public StudentThesisGroupOutput StudentThesisGroup { get; set; }
        public List<StudentOutput> Students { get; set; }
    }
}
