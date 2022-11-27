using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Doresearch
    {
        public string PkLecturersId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int DoresearchQuantiity { get; set; }
        public string PkResearchId { get; set; }
    }
}
