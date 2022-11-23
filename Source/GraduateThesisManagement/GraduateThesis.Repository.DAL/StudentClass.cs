using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class StudentClass
    {
        public StudentClass()
        {
            Students = new HashSet<Student>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StudentQuantity { get; set; }
        public string Notes { get; set; }
        public string PkFacultyId { get; set; }
        public string PkCourseTrainingId { get; set; }

        public virtual CourseTraining PkCourseTraining { get; set; }
        public virtual Faculty PkFaculty { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
