using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Student
    {
        public Student()
        {
            StudentThesisGroups = new HashSet<StudentThesisGroup>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public string Notes { get; set; }
        public string PkStudentClassId { get; set; }
        public string PkThesisId { get; set; }
        public string Sex { get; set; }

        public virtual StudentClass PkStudentClass { get; set; }
        public virtual Thesis PkThesis { get; set; }
        public virtual ICollection<StudentThesisGroup> StudentThesisGroups { get; set; }
    }
}
