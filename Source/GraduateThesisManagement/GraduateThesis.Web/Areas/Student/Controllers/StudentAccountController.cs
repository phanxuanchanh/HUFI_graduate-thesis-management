using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            string pageName = "Trang đăng nhập dành cho sinh viên";
            if (ModelState.IsValid)
            {
                SignInResultModel signInResultModel = await _studentRepository.SignInAsync(signInModel);

                if (signInResultModel.Status == SignInStatus.Success)
                    return RedirectToAction("Index");

                AddTempData(pageName, signInResultModel);
                return RedirectToAction("LoadSignInView");
            }

            AddTempData(pageName, SignInStatus.InvalidData);
            return RedirectToAction("LoadSignInView");
        }

        [Route("forgot-password-view")]
        public IActionResult ForgotPasswordView()
        {
            return View();
        }
    }
}
