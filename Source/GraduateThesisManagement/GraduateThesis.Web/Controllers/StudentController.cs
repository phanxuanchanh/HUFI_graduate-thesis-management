using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IStudentClassRepository _studentClassRepository;

        public StudentController(IRepository repository)
        {
            _studentRepository = repository.StudentRepository;
            _studentClassRepository = repository.StudentClassRepository;

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
        public async Task<ActionResult> Create()
        {

            List<StudentClassOutput> studentClasses  = await _studentClassRepository.GetListAsync();
                ViewBag.StudentClassList = new SelectList(studentClasses, "ID", "Name");
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentInput studentInput)
        {
            if (ModelState.IsValid)
            {
                List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync();
                ViewBag.StudentClassList = new SelectList(studentClasses, "ID", "Name");

                DataResponse<StudentOutput> dataResponse = _studentRepository.Create(studentInput);
                TempData["Status"] = dataResponse.Status.ToString();
                if (dataResponse.Status == DataResponseStatus.Success)
                {
                    TempData["statusCode"] = "Success";
                    TempData["status"] = "Đã thêm thành công";
                    return View(studentInput);
                }
                else if (dataResponse.Status == DataResponseStatus.AlreadyExists)
                {
                    TempData["Messgae"] = "Đã tồn tại";
                    return View(studentInput);
                }
                else
                {
                    TempData["Messgae"] = "Thêm không thành công, lỗi không xác định";

                }

            }
            TempData["Status"] = "InvaliData";
            return View(studentInput);
        }
        [Route("edit")]
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");
            StudentOutput studentOutput = await _studentRepository.GetAsync(id);
            if (studentOutput == null)
                return RedirectToAction("Index");
            return View(studentOutput);
        }

        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, StudentInput studentInput)
        {
            if (ModelState.IsValid)
            {
                DataResponse<StudentOutput> dataResponse = await _studentRepository.UpdateAsync(id, studentInput);
                TempData["Status"] = dataResponse.Status.ToString();
                if (dataResponse.Status == DataResponseStatus.Success)
                {
                    TempData["statusCode"] = "Success";
                    TempData["status"] = "Đã thêm thành công";
                    return View(studentInput);
                }
                else if (dataResponse.Status == DataResponseStatus.AlreadyExists)
                {
                    TempData["Messgae"] = "Đã tồn tại";
                    return View(studentInput);
                }
                else
                {
                    TempData["Messgae"] = "Thêm không thành công, lỗi không xác định";

                }

            }
            TempData["Status"] = "InvaliData";

            return View(studentInput);
        }

        [HttpGet]
        public async Task<ActionResult> Details(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
                return RedirectToAction("Index");
            StudentOutput studentOutput = await _studentRepository.GetAsync(studentId);
            if (studentOutput == null)
                return RedirectToAction("Index");
            return View(studentOutput);

        }

        [Route("delete")]
        [HttpPost]

        public async Task<ActionResult> Delete(string studentId)
        {
            try
            {
                DataResponse dataResponse = await _studentRepository.BatchDeleteAsync(studentId);
                if (dataResponse.Status == DataResponseStatus.Success)
                {
                    TempData["statusCode"] = "Success";
                    TempData["status"] = "Đã xóa thành công";
                    return RedirectToAction("Index");
                }
                else if (dataResponse.Status == DataResponseStatus.Failed)
                {
                    TempData["statusCode"] = "Failed";
                    TempData["status"] = "Có lỗi khi xóa";
                    return RedirectToAction("Index");
                }
                else if (dataResponse.Status == DataResponseStatus.AlreadyExists)
                {
                    TempData["statusCode"] = "Failed";
                    TempData["status"] = "Không tìm thấy dữ liệu!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["statusCode"] = "Failed";
                    TempData["status"] = "Lỗi không xác định!";
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                return View(viewName: "Error", model: ex.Message);
            }
        }

    }

}
