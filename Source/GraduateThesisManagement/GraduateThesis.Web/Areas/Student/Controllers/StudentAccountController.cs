using GraduateThesis.Common.Authorization;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GraduateThesis.Web.Areas.Student.Controllers
{
    [Area("Student")]
    [Route("student/account")]
    [HandleException]
    public class StudentAccountController : WebControllerBase
    {
        private IStudentRepository _studentRepository;

        public StudentAccountController(IRepository repository)
        {
            _studentRepository = repository.StudentRepository;
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
    }
}
