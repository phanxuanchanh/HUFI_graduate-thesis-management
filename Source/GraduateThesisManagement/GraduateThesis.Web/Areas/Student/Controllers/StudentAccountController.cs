using GraduateThesis.Common.Authorization;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
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
        [PageName(Name = "Trang đăng nhập dành cho sinh viên")]
        public IActionResult LoadSignInView()
        {
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
                {
                    StudentOutput student =  await _studentRepository.GetAsync(signInModel.Code);
                    string accountSession = JsonConvert.SerializeObject(new AccountSession
                    {
                        AccountModel = student,
                        Role = "Student",
                        LastSignInTime = DateTime.Now
                    });

                    HttpContext.Session.SetString("account-session", accountSession);

                    return RedirectToAction("Index", "StudentThesis");
                }

                AddTempData(pageName, signInResultModel);
                return RedirectToAction("LoadSignInView");
            }

            AddTempData(pageName, SignInStatus.InvalidData);
            return RedirectToAction("LoadSignInView");
        }

        [Route("forgot-password-view")]
        [HttpGet]
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
