using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Faculty
    {
        public Faculty()
        {
            FacultyStaffs = new HashSet<FacultyStaff>();
            StudentClasses = new HashSet<StudentClass>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Dean { get; set; }

        public virtual ICollection<FacultyStaff> FacultyStaffs { get; set; }
        public virtual ICollection<StudentClass> StudentClasses { get; set; }
    }
}
