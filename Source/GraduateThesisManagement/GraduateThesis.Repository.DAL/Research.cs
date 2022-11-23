using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Research
    {
        public Research()
        {
            Doresearches = new HashSet<Doresearch>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<Doresearch> Doresearches { get; set; }
    }
}
