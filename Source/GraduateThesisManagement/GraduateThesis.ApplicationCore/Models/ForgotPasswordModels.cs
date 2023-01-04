﻿using System.ComponentModel.DataAnnotations;
using GraduateThesis.ApplicationCore.Enums;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class ForgotPasswordModel
{
    public AccountStatus AccountStatus { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}