using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/account")]
    public class LectureAccountController : Controller
    {
        [Route("sign-in-view")]
        [HttpGet]
        [PageName(Name = "Trang đăng nhập dành cho giảng viên,...")]
        public IActionResult LoadSignInView()
        {
            return View();
        }

        [Route("sign-in")]
        [HttpPost]
        public IActionResult SignIn(SignInModel signInModel)
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View(viewName: "_Error");
            }
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
