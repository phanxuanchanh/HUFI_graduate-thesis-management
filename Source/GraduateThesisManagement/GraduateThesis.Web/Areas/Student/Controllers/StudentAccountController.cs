﻿using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Student.Controllers;

[Area("Student")]
[Route("student/account")]
public class StudentAccountController : WebControllerBase
{
    private IStudentRepository _studentRepository;
    private IThesisGroupRepository _studentThesisGroupRepository;
    private IEmailService _emailService;
    private IAccountManager _accountManager;

    public StudentAccountController(IRepository repository, IEmailService emailService, IAccountManager accountManager)
    {
        _studentRepository = repository.StudentRepository;
        _studentThesisGroupRepository = repository.ThesisGroupRepository;
        _emailService = emailService;
        _accountManager = accountManager;

        _studentRepository.EmailService = emailService;
    }

    [Route("sign-in")]
    [HttpGet]
    [PageName(Name = "Trang đăng nhập dành cho sinh viên")]
    public IActionResult SignIn()
    {
        return View(new SignInModel());
    }

    [Route("sign-in")]
    [HttpPost]
    [PageName(Name = "Trang đăng nhập dành cho sinh viên")]
    public async Task<IActionResult> SignIn(SignInModel signInModel)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(AccountStatus.InvalidData);
            return View(signInModel);
        }

        SignInResultModel signInResultModel = await _studentRepository.SignInAsync(signInModel);

        if (signInResultModel.Status == AccountStatus.Success)
        {
            StudentOutput student = await _studentRepository.GetAsync(signInModel.Code);
            if (string.IsNullOrEmpty(student.Avatar))
                student.Avatar = "default-male-profile.png";

            _accountManager.SetHttpContext(HttpContext);
            _accountManager.SetSession(new AccountSession
            {
                AccountModel = student,
                Roles = "Student",
                LastSignInTime = DateTime.Now
            });

            return RedirectToAction("Index", "StudentThesis");
        }

        AddViewData(signInResultModel);
        return View(signInModel);
    }

    [Route("forgot-password")]
    [HttpGet]
    [PageName(Name = "Lấy lại mật khẩu")]
    public IActionResult ForgotPassword()
    {
        return View(new ForgotPasswordModel());
    }

    [Route("forgot-password")]
    [HttpPost]
    [PageName(Name = "Lấy lại mật khẩu")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
    {
        if (!ModelState.IsValid)
        {
            AddViewData(AccountStatus.InvalidData);
            return View(forgotPasswordModel);
        }

        AccountVerificationModel accountVerification = await _studentRepository
            .ForgotPasswordAsync(forgotPasswordModel);

        if (accountVerification.Status == AccountStatus.Success)
        {
            AddViewData(accountVerification);
            return View("Verify", accountVerification);
        }   

        AddViewData(accountVerification);
        return View(forgotPasswordModel);
    }

    [Route("verify")]
    [HttpPost]
    [PageName(Name = "Lấy lại mật khẩu")]
    public async Task<IActionResult> Verify(AccountVerificationModel verificationModel)
    {
        if (!ModelState.IsValid)
        {
            AddViewData(AccountStatus.InvalidData);
            return View(verificationModel);
        }

        NewPasswordModel newPasswordModel = await _studentRepository
            .VerifyAccountAsync(verificationModel);

        if (newPasswordModel.Status == AccountStatus.Success)
        {
            AddViewData(newPasswordModel);
            return View("CreatePassword", newPasswordModel);
        }

        AddViewData(newPasswordModel);
        return View(verificationModel);
    }

    [Route("create-password")]
    [HttpPost]
    [PageName(Name = "Lấy lại mật khẩu")]
    public async Task<IActionResult> CreatePassword(NewPasswordModel newPasswordModel)
    {
        if (!ModelState.IsValid)
        {
            AddViewData(AccountStatus.InvalidData);
            return View(newPasswordModel);
        }

        ForgotPasswordModel forgotPasswordModel = await _studentRepository
            .CreateNewPasswordAsync(newPasswordModel);

        if (forgotPasswordModel.Status == AccountStatus.Success)
        {
            AddTempData(forgotPasswordModel);
            return RedirectToAction("SignIn");
        }

        AddViewData(forgotPasswordModel);
        return View(newPasswordModel);
    }

    [Route("sign-out")]
    [HttpGet]
    public IActionResult SignOutAccount()
    {
        HttpContext.Session.SetString("account-session", "");
        return RedirectToAction("Index", "Home");
    }

    [Route("approved-studentThesisGroup")]
    [HttpPost]
    [PageName(Name = "Vào nhóm đề tài")]
    public async Task<IActionResult> ApprovalStudentThesisGroupAsync([Required] string StudentThesisGroupId)
    {
        DataResponse dataResponse = await _studentThesisGroupRepository.ApprovalStudentThesisGroupAsync(StudentThesisGroupId);
        AddTempData(dataResponse);
        return RedirectToAction("YourStudentThesisGroup");
    }


    [Route("refuseapproved-studentThesisGroup")]
    [HttpPost]
    [PageName(Name = "Từ chối vào nhóm đề tài")]
    public async Task<IActionResult> RefuseApprovalStudentThesisGroupAsync([Required] string StudentThesisGroupId)
    {
        DataResponse dataResponse = await _studentThesisGroupRepository.RefuseApprovalStudentThesisGroupAsync(StudentThesisGroupId);
        AddTempData(dataResponse);
        return RedirectToAction("YourStudentThesisGroup");
    }

    [Route("profiles-in-view")]
    [HttpGet]
    [PageName(Name = "Thông tin sinh viên")]
    public IActionResult ProfilesView()
    {
        return View();
    }
}
