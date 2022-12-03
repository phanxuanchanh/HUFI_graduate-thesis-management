using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GraduateThesis.Web.Controllers
{
    [Route("student")]
    public class StudentController : Controller
    {

        private readonly IStudentRepository _studentRepository;
        public StudentController(IRepository repository)
        {
            _studentRepository = repository.StudentRepository;
        }
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            return View(await _studentRepository.GetListAsync());
        }

        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [Route("Create")]
        public ActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentInput studentInput)
        {
            if (ModelState.IsValid)
            {
                DataResponse<StudentOutput> dataResponse = _studentRepository.Create(studentInput);
                ViewData["Status"] = dataResponse.Status.ToString();
                if (dataResponse.Status == DataResponseStatus.Success)
                {
                    ViewData["Messgae"] = "Đã thêm thành công!";
                    return View(studentInput);
                }
                else if (dataResponse.Status == DataResponseStatus.AlreadyExists)
                {
                    ViewData["Messgae"] = "Đã tồn tại";
                    return View(studentInput);
                }
                else
                {
                    ViewData["Messgae"] = "Thêm không thành công, lỗi không xác định";

                }

            }
            ViewData["Status"] = "InvaliData";
            return View(studentInput);
        }
        [Route("Details/{studentId}")]
        [HttpGet]
        public async Task<ActionResult> Details(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
                return RedirectToAction("Index");
            StudentOutput studentOutput = await _studentRepository.GetAsync(studentId);
            if (studentId == null)
                return RedirectToAction("Index");
            return View(studentId);

        }
     
       }
    
}
