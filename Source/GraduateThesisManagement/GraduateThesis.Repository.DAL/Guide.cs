﻿using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Guide
    {
        public string PkThesisId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int GuidePoint { get; set; }
        public string Notes { get; set; }
        public string Comment { get; set; }
        public string PkLecturersId { get; set; }

        public virtual Lecturer PkThesis { get; set; }
        public virtual Thesis PkThesisNavigation { get; set; }
    }
}
