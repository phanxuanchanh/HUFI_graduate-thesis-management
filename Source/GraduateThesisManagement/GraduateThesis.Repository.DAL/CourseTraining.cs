﻿using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class CourseTraining
    {
        public CourseTraining()
        {
            StudentClasses = new HashSet<StudentClass>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int ShoolYear { get; set; }

        public virtual ICollection<StudentClass> StudentClasses { get; set; }
    }
}
