using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Student.Controllers
{
    [Area("Student")]
    [Route("student/account")]
    public class StudentAccountController : WebControllerBase
    {
        private IStudentRepository _studentRepository;
        private IThesisGroupRepository _studentThesisGroupRepository;


        public StudentAccountController(IRepository repository)
        {
            _studentRepository = repository.StudentRepository;
            _studentThesisGroupRepository = repository.ThesisGroupRepository;

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

                string accountSession = JsonConvert.SerializeObject(new AccountSession
                {
                    AccountModel = student,
                    Role = "Student",
                    LastSignInTime = DateTime.Now
                });

                HttpContext.Session.SetString("account-session", accountSession);

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
        [WebAuthorize(AccountRole.Student)]
        public async Task<IActionResult> ApprovalStudentThesisGroupAsync([Required] string StudentThesisGroupId)
        {
            DataResponse dataResponse = await _studentThesisGroupRepository.ApprovalStudentThesisGroupAsync(StudentThesisGroupId);
            AddTempData(dataResponse);
            return RedirectToAction("YourStudentThesisGroup");
        }


        [Route("refuseapproved-studentThesisGroup")]
        [HttpPost]
        [PageName(Name = "Từ chối vào nhóm đề tài")]
        [WebAuthorize(AccountRole.Student)]
        public async Task<IActionResult> RefuseApprovalStudentThesisGroupAsync([Required] string StudentThesisGroupId)
        {
            DataResponse dataResponse = await _studentThesisGroupRepository.RefuseApprovalStudentThesisGroupAsync(StudentThesisGroupId);
            AddTempData(dataResponse);
            return RedirectToAction("YourStudentThesisGroup");
        }
    }
}
