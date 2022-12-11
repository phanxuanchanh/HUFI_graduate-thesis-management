using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class StudentThesisGroupDetail
    {
        public string StudentThesisGroupId { get; set; }
        public string StudentId { get; set; }
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Student Student { get; set; }
        public virtual StudentThesisGroup StudentThesisGroup { get; set; }
    }
}
