using System;

namespace GraduateThesis.Repository.DTO
{
    public class FacultyStaffInput
    {
        public string Id { get; set; }
        public string FacultyId { get; set; }
        public string FacultyRoleId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public string Position { get; set; }
        public string Degree { get; set; }
        public string Notes { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }

    public class FacultyStaffOutput : FacultyStaffInput
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public FacultyOutput Faculty { get; set; }
        public AppRolesInput FacultyStaffRole { get; set; }
    }

}
