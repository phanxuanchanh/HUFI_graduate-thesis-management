using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Counterargument
    {
        public string PkThesisId { get; set; }
        public string PkLecturersId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Comment { get; set; }
        public int Feedbackpoints { get; set; }

        public virtual Lecturer PkLecturers { get; set; }
        public virtual Thesis PkThesis { get; set; }
    }
}
