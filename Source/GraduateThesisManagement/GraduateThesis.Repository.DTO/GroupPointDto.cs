using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.DTO;

public class GroupPointInput
{
    public string GroupId { get; set; }
    public List<StudentPointInput> StudentPoints { get; set; }
}
