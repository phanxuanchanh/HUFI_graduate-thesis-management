using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class Council
    {
        public Council()
        {
            Theses = new HashSet<Thesis>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Notes { get; set; }
        public string Chairman { get; set; }
        public double CouncilPoint { get; set; }
        public int Numberofmember { get; set; }

        public virtual ICollection<Thesis> Theses { get; set; }
    }
}
