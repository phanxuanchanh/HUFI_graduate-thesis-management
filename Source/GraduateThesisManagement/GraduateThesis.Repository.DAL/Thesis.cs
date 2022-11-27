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
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxStudentNumber { get; set; }
        public string SourceCode { get; set; }
        public string GeneralComment { get; set; }
        public int Year { get; set; }
        public string Notes { get; set; }
        public string TopicId { get; set; }
        public string CouncilId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Council Council { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual ICollection<StudentThesisGroup> StudentThesisGroups { get; set; }
    }
}
