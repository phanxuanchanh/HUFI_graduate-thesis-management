﻿using System;
using System.Collections.Generic;

namespace GraduateThesis.Repository.DAL;

public partial class FacultyStaff
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

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<CommitteeMember> CommitteeMembers { get; } = new List<CommitteeMember>();

    public virtual ICollection<CounterArgumentResult> CounterArgumentResults { get; } = new List<CounterArgumentResult>();

    public virtual Faculty Faculty { get; set; }

    public virtual AppRoles FacultyRole { get; set; }

    public virtual ICollection<Thesis> Theses { get; } = new List<Thesis>();

    public virtual ICollection<ThesisSupervisor> ThesisSupervisors { get; } = new List<ThesisSupervisor>();
}
