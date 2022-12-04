using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Student.Controllers
{
    [Area("Student")]
    [Route("student/account")]
    public class StudentAccountController : WebControllerBase
    {
        private IStudentRepository _studentRepository;

        public StudentAccountController(IRepository repository)
        {
            _studentRepository = repository.StudentRepository;
        }

        [Route("sign-in-view")]
        [HttpGet]
        public IActionResult LoadSignInView()
        {
            AddViewData("Trang đăng nhập dành cho sinh viên");
            return View(new SignInModel());
        }

        [Route("sign-in")]
        [HttpPost]
        public IActionResult SignIn(SignInModel signInModel)
        {
            string pageName = "Trang đăng nhập dành cho sinh viên";
            if (ModelState.IsValid)
            {
                AddViewData(pageName, "Success", "Dữ liệu nhập vào không hợp lệ!");
                return View();
            }

            AddViewData(pageName, "InvalidData", "Dữ liệu nhập vào không hợp lệ!");
            return View(signInModel);
        }

        [Route("forgot-password-view")]
        public IActionResult ForgotPasswordView()
        {
            return View();
        }
    }
}
