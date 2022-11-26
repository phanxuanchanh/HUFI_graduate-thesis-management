using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SignInStudent()
        {
            return View();
        }
        public IActionResult ResetPasswordStudent()
        {
            return View();
        }
        public IActionResult SignInLecturerst()
        {
            return View();
        }
        public IActionResult ResetPasswordLecturers()
        {
            return View();
        }
    }
}
