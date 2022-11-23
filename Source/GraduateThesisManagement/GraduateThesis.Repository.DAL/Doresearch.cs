using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Doresearch
    {
        public string PkLecturersId { get; set; }
        public string Notes { get; set; }
        public int DoresearchQuantiity { get; set; }
        public string PkResearchId { get; set; }

        public virtual Lecturer PkLecturers { get; set; }
        public virtual Research PkResearch { get; set; }
    }
}
