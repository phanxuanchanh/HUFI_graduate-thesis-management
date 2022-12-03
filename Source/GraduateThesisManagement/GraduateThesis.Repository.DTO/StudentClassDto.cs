using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.DTO
{
    public class StudentClassInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StudentQuantity { get; set; }
        public string Notes { get; set; }
    }

    public class StudentClassOutput : StudentClassInput
    {
     
    }
}
