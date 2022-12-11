using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/student-manager")]
    public class StudentManagerController: WebControllerBase
    {
        public string PageName { get; set; }

        private IStudentRepository _studentRepository;

        public StudentManagerController(IRepository repository)
        {
            _studentRepository = repository.StudentRepository;
        }

        [Route("list")]        
        [HttpGet]        
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = null, string orderOptions = "ASC", string keyword = null)
        {
            try
            {
                Pagination<StudentOutput> pagination;
                if (orderOptions == "ASC")
                    pagination = await _studentRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);
                else
                    pagination = await _studentRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.DESC, keyword);

                StaticPagedList<StudentOutput> pagedList = pagination.ToStaticPagedList();
                ViewData["PagedList"] = pagedList;

                return View();
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details([Required] string id)
        {
            try
            {
                StudentOutput studentOutput = await _studentRepository.GetAsync(id);
                if (studentOutput == null)
                    return RedirectToAction("Index");

                return View(studentOutput);
            }
            catch
            {
                return View(viewName: "_Error");
            }
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(StudentInput studentInput)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DataResponse<StudentOutput> dataResponse = await _studentRepository.CreateAsync(studentInput);

                    AddViewData(PageName, dataResponse);
                    return View(studentInput);
                }

                AddViewData(PageName, DataResponseStatus.InvalidData);
                return View(studentInput);
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit([Required] string id)
        {
            try
            {
                StudentOutput studentOutput = await _studentRepository.GetAsync(id);
                if (studentOutput == null)
                    return RedirectToAction("Index");

                return View(studentOutput);
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit([Required] string id, StudentInput studentInput)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    StudentOutput studentOutput = await _studentRepository.GetAsync(id);
                    if (studentOutput == null)
                        return RedirectToAction("Index");

                    DataResponse<StudentOutput> dataResponse = await _studentRepository.UpdateAsync(id, studentInput);
                    
                    AddViewData(PageName, dataResponse);
                    return View(studentInput);
                }

                AddViewData(PageName, DataResponseStatus.InvalidData);
                return View(studentInput);
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete([Required] string id)
        {
            try
            {
                StudentOutput studentOutput = await _studentRepository.GetAsync(id);
                if (studentOutput == null)
                    return RedirectToAction("Index");

                DataResponse dataResponse = await _studentRepository.BatchDeleteAsync(id);
                
                AddTempData(PageName, dataResponse);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

    }
}
