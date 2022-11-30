using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Implements;
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
        private IStudentRepository _studentRepository;

        public StudentController(IRepository repository)
        {
            _studentRepository = repository.StudentRepository;
        }

        //[Route("list")]
        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _studentRepository.GetListAsync());
        //}

        [Route("details")]
        [HttpGet]
        public async Task<IActionResult> GetDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            StudentOutput student = await _studentRepository.GetAsync(id);
            if(student == null)
                return RedirectToAction("Index");

            return View(student);
        }

        //[Route("create")]
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[Route("create")]
        //[HttpPost]
        //public async Task<IActionResult> Create(StudentInput student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        DataResponse<StudentOutput> dataResponse = await _studentRepository.CreateAsync(student);
        //        ViewData["Status"] = dataResponse.Status.ToString();
        //        if (dataResponse.Status == DataResponseStatus.Success)
        //        {
        //            ViewData["Message"] = "Đã thêm thành công!";
        //            return View(student);
        //        }else if(dataResponse.Status == DataResponseStatus.AlreadyExists)
        //        {
        //            ViewData["Message"] = "Đã thêm thành công!";
        //            return View(student);
        //        }
        //        else
        //        {
        //            ViewData["Message"] = "Thêm không thành công, lỗi không xác định!";
        //        }
        //    }
        //    ViewData["Status"] = "InvalidData";
        //    ViewData["Message"] = "Nhập dữ liệu không hợp lệ!";
        //    return View(student);
        //}


        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
