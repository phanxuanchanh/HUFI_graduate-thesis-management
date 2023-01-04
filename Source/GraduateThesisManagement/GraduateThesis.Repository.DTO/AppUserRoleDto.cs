using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class AppUserRoleInput
{
    [Display(Name = "Mã người dùng")]
    public string UserId { get; set; }

    [Display(Name = "Mã chức danh")]
    public string RoleId { get; set; }

    [Display(Name = "Các mô tả, yêu cầu")]
    public string Description { get; set; }


    [Display(Name = "Ghi chú")]
    public string Notes { get; set; }
}

public class AppUserRoleOutput : AppUserRoleInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public AppRoleOutput AppRoles { get; set; }
    public FacultyOutput Faculty { get; set; }
}
