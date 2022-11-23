using System;
using System.Collections.Generic;

#nullable disable

namespace GraduateThesis.Repository.DAL
{
    public partial class StudentThesisGroupDetail
    {
        public string PkStudentThesisGroupId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }

        public virtual StudentThesisGroup PkStudentThesisGroup { get; set; }
    }
}
