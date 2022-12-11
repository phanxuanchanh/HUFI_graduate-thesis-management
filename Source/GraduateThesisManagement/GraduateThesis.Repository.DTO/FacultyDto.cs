using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.DTO
{
    public class FacultyInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class FacultyOutput : FacultyInput
    {
        public List<FacultyStaffOutput> FacultyStaffs { get; set; }
        public List<StudentClassOutput> StudentClasses { get; set; }
    }
}
