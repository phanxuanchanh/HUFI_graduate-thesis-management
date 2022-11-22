using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Lecturer
    {
        public Lecturer()
        {
            Counterarguments = new HashSet<Counterargument>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int Phone { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public string Position { get; set; }
        public string Degree { get; set; }
        public string Notes { get; set; }

        public virtual Doresearch Doresearch { get; set; }
        public virtual Guide Guide { get; set; }
        public virtual ICollection<Counterargument> Counterarguments { get; set; }
    }
}
