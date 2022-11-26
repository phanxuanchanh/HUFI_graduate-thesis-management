using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class StudentThesisGroup
    {
        public StudentThesisGroup()
        {
            StudentThesisGroupDetails = new HashSet<StudentThesisGroupDetail>();
        }

        public string Id { get; set; }
        public string ThesisId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StudentQuantity { get; set; }
        public string Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Thesis Thesis { get; set; }
        public virtual ICollection<StudentThesisGroupDetail> StudentThesisGroupDetails { get; set; }
    }
}
