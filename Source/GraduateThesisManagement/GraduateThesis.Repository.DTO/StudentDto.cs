using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO
{
    public class StudentInput
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public string Notes { get; set; }
        public string Sex { get; set; }
    }

    public class StudentOutput : StudentInput
    {
        public StudentClassOutput StudentClass { get; set; }
    }
}
