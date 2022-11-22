using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Thesis
    {
        public Thesis()
        {
            StudentThesisGroups = new HashSet<StudentThesisGroup>();
            Students = new HashSet<Student>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxStudentNumber { get; set; }
        public string SourceCode { get; set; }
        public string Generalcommet { get; set; }
        public int Year { get; set; }
        public string Notes { get; set; }
        public string PkTopicId { get; set; }
        public string PkCouncilId { get; set; }

        public virtual Council PkCouncil { get; set; }
        public virtual Topic PkTopic { get; set; }
        public virtual Counterargument Counterargument { get; set; }
        public virtual Guide Guide { get; set; }
        public virtual ICollection<StudentThesisGroup> StudentThesisGroups { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
