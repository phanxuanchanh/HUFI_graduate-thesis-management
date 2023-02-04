using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DAL;

public partial class ThesisStatus
{
    public int Id { get; set; }

    [Display(Name = "Trạng thái")]
    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Thesis> Theses { get; } = new List<Thesis>();
}
