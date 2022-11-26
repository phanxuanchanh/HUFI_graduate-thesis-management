﻿using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Student
    {
        public Student()
        {
            StudentThesisGroupDetails = new HashSet<StudentThesisGroupDetail>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public string Notes { get; set; }
        public string StudentClassId { get; set; }
        public string CourseTrainingId { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual StudentClass StudentClass { get; set; }
        public virtual ICollection<StudentThesisGroupDetail> StudentThesisGroupDetails { get; set; }
    }
}
