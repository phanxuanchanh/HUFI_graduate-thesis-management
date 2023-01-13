using GraduateThesis.ApplicationCore.AppController;
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

    [Route("sign-in-view")]
    [HttpGet]
    [PageName(Name = "Trang đăng nhập dành cho sinh viên")]
    public IActionResult LoadSignInView()
    {
        return View(new SignInModel());
    }

    [Route("sign-in")]
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInModel signInModel)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(SignInStatus.InvalidData);
            return RedirectToAction("LoadSignInView");
        }

        SignInResultModel signInResultModel = await _studentRepository.SignInAsync(signInModel);

        if (signInResultModel.Status == SignInStatus.Success)
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

        AddTempData(signInResultModel);
        return RedirectToAction("LoadSignInView");
    }

    [Route("forgot-password-view")]
    [HttpGet]
    [PageName(Name = "Trang lấy lại mật khẩu sinh viên")]
    public IActionResult ForgotPasswordView()
    {
        return View();
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
