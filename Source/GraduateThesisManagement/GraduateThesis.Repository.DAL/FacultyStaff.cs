using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class FacultyStaff
    {
        public string Id { get; set; }
        public string FacultyId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
    }
}
