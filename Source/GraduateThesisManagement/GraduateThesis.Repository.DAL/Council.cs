using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Council
    {
        public Council()
        {
            ThesisCommittees = new HashSet<ThesisCommittee>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Semester { get; set; }
        public string Year { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<ThesisCommittee> ThesisCommittees { get; set; }
    }
}
