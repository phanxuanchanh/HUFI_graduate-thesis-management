using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Lecturer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sex { get; set; }
        public int Phone { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public string Position { get; set; }
        public string Degree { get; set; }
        public string Notes { get; set; }
    }
}
