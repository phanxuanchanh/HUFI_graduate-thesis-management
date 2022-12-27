using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class AppUserRole
{
    public string UserId { get; set; }

    public string RoleId { get; set; }

    public string Description { get; set; }

    public string Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual AppRole Role { get; set; }

    public virtual Faculty User { get; set; }
}
