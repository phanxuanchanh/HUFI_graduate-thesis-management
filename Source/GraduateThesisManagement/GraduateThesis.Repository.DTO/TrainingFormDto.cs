using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.DTO
{
    public class TrainingFormInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
 
    }


    public class TrainingFormOutput : TrainingFormInput
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

       
    }
}
