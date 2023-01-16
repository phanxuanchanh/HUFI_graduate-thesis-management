using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/account")]
public class FacultyStaffAccountController : WebControllerBase
{
    private IFacultyStaffRepository _facultyStaffRepository;
    private IThesisRepository _thesisRepository;
    private IAccountManager _accountManager;

    public FacultyStaffAccountController(IRepository repository, IEmailService emailService, IAccountManager accountManager)
    {
        _facultyStaffRepository = repository.FacultyStaffRepository;
        _thesisRepository = repository.ThesisRepository;
        _accountManager = accountManager;

        _facultyStaffRepository.EmailService = emailService;
    }

    [Route("sign-in")]
    [HttpGet]
    [PageName(Name = "Trang đăng nhập dành cho giảng viên")]
    public IActionResult SignIn()
    {
        return View(new SignInModel());
    }

    [Route("sign-in")]
    [HttpPost]
    [PageName(Name = "Trang đăng nhập dành cho giảng viên")]
    public async Task<IActionResult> SignIn(SignInModel signInModel)
    {
        if (!ModelState.IsValid)
        {
            AddViewData(AccountStatus.InvalidData);
            return View(signInModel);
        }

        SignInResultModel signInResultModel = await _facultyStaffRepository.SignInAsync(signInModel);

        if (signInResultModel.Status == AccountStatus.Success)
        {
            FacultyStaffOutput facultyStaff = await _facultyStaffRepository.GetAsync(signInModel.Code);
            _accountManager.SetHttpContext(HttpContext);

            _accountManager.SetSession(new AccountSession
            {
                UserId = facultyStaff.Id,
                LastSignInTime = DateTime.Now,
                AccountModel = facultyStaff
            });

            return RedirectToAction("Index", "LectureDashboard");
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

        AccountVerificationModel accountVerification = await _facultyStaffRepository
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

        NewPasswordModel newPasswordModel = await _facultyStaffRepository
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

        ForgotPasswordModel forgotPasswordModel = await _facultyStaffRepository
            .CreateNewPasswordAsync(newPasswordModel);

        if(forgotPasswordModel.Status == AccountStatus.Success)
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
        _accountManager.SetHttpContext(HttpContext);
        _accountManager.RemoveSession();
        return RedirectToAction("Index", "Home");
    }

    [Route("get-profile")]
    [HttpGet]
    [WebAuthorize]
    [AccountInfo(typeof(FacultyStaffOutput))]
    [PageName(Name = "Thông tin giảng viên")]
    public async Task<IActionResult> GetProfile()
    {
        _accountManager.SetHttpContext(HttpContext);
        AccountSession accountSession = _accountManager.GetSession();
        FacultyStaffOutput facultyStaffOutput = await _facultyStaffRepository.GetAsync(accountSession.UserId);

        if (string.IsNullOrEmpty(facultyStaffOutput.Avatar))
            facultyStaffOutput.Avatar = "default-male-profile.png";

        return View(facultyStaffOutput);
    }

    [Route("update-profile")]
    [HttpPost]
    [WebAuthorize]
    [AccountInfo(typeof(FacultyStaffOutput))]
    public IActionResult UpdateProfile(IFormFile formFile, FacultyStaffInput facultyStaffInput)
    {
        return View();
    }
}
