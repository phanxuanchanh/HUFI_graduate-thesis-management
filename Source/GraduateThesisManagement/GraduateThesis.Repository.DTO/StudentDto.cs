using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO
{
    public class StudentInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string StudentClassId { get; set; }
        public string Password { get; set; }
    }

    public class StudentOutput : StudentInput
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public StudentClassOutput StudentClass { get; set; }
        public List<StudentThesisGroupOutput> StudentThesisGroups { get; set; }
    }
}
