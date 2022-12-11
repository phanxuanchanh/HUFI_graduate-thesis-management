﻿using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class StudentThesisGroup
    {
        public StudentThesisGroup()
        {
            StudentThesisGroupDetails = new HashSet<StudentThesisGroupDetail>();
            Theses = new HashSet<Thesis>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StudentQuantity { get; set; }
        public string Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<StudentThesisGroupDetail> StudentThesisGroupDetails { get; set; }
        public virtual ICollection<Thesis> Theses { get; set; }
    }
}
