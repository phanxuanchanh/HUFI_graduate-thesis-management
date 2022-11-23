using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class FacultyStaff
    {
        public string Id { get; set; }
        public string PkFacultyId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public virtual Faculty PkFaculty { get; set; }
    }
}
