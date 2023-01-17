﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Repository.DTO;

public class StudentInput
{
    [Display(Name = "Mã sinh viên")]
    public string Id { get; set; }

    [Display(Name = "Tên sinh viên")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Name { get; set; }


    [Display(Name = "Số điện thoại")]
    [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Độ dài điện thoại từ 10-11 kí tự")]
    [Phone(ErrorMessage = "{0} không hợp lệ")]
    public string Phone { get; set; }

    [Display(Name = "Email")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [EmailAddress(ErrorMessage = "{0} không hợp lệ")]
    public string Email { get; set; }

    [Display(Name = "Đia chỉ")]
    public string Address { get; set; }

    [Display(Name = "Ngày sinh")]
    [DataType(DataType.Date, ErrorMessage = "{0} có kiểu dữ liệu không hợp lệ")]
    public DateTime Birthday { get; set; }

    [Display(Name = "Ảnh nhân viên")]
    public string Avatar { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; }

    [Display(Name = "Mã lớp")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string StudentClassId { get; set; }

    [Display(Name = "Giới tính")]
    [RegularExpression("Nam|Nữ", ErrorMessage = "{0} chỉ có 'Nam' hoặc 'Nữ'")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Gender { get; set; }
}

public class StudentOutput : StudentInput
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public StudentClassOutput StudentClass { get; set; }
    public List<ThesisGroupOutput> ThesisGroups { get; set; }
}
