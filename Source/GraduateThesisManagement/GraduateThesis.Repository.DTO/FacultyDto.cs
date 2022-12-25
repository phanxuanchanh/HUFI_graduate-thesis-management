using System;
using System.Collections.Generic;

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
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<FacultyStaffOutput> FacultyStaffs { get; set; }
        public List<StudentClassOutput> StudentClasses { get; set; }

        public FacultyStaffRoleOutput FacultyStaffRole { get; set; }
    }
}
