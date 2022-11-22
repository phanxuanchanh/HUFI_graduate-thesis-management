using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class StudentThesisGroup
    {
        public string Id { get; set; }
        public string PkStudentsId { get; set; }
        public string PkThesisId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StudentQuantity { get; set; }
        public string Notes { get; set; }

        public virtual Student PkStudents { get; set; }
        public virtual Thesis PkThesis { get; set; }
        public virtual StudentThesisGroupDetail StudentThesisGroupDetail { get; set; }
    }
}
