using GraduateThesis.Common.Authorization;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/account")]
    [HandleException]
    public class LectureAccountController : WebControllerBase
    {
        private IFacultyStaffRepository _facultyStaffRepository;

        public LectureAccountController(IRepository repository)
        {
            _facultyStaffRepository = repository.FacultyStaffRepository;
        }

        [Route("sign-in-view")]
        [HttpGet]
        [PageName(Name = "Trang đăng nhập dành cho giảng viên")]
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

            SignInResultModel signInResultModel = await _facultyStaffRepository.SignInAsync(signInModel);

            if (signInResultModel.Status == SignInStatus.Success)
            {
                FacultyStaffOutput facultyStaff = await _facultyStaffRepository.GetAsync(signInModel.Code);
                string accountSession = JsonConvert.SerializeObject(new AccountSession
                {
                    AccountModel = facultyStaff,
                    Role = facultyStaff.FacultyStaffRole.Name,
                    LastSignInTime = DateTime.Now
                });

                HttpContext.Session.SetString("account-session", accountSession);

                return RedirectToAction("Index", "LectureDashboard");
            }

            AddTempData(signInResultModel);
            return RedirectToAction("LoadSignInView");
        }

        [Route("forgot-password-view")]
        [HttpGet]
        [PageName(Name = "Lấy lại mật khẩu")]
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
