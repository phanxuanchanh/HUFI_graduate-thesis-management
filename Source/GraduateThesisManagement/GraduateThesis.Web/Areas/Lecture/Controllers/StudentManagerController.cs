using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/student-manager")]
    public class StudentManagerController : WebControllerBase
    {
        public string PageName { get; set; } = "Quản lý sinh viên";

        private IStudentRepository _studentRepository;
        private IStudentClassRepository _studentClassRepository;


        public StudentManagerController(IRepository repository)
        {
            _studentRepository = repository.StudentRepository;
            _studentClassRepository = repository.StudentClassRepository;
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
                ViewData["OrderBy"] = orderBy;
                ViewData["OrderOptions"] = orderOptions;
                ViewData["Keyword"] = keyword;

                AddViewData(PageName);

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

                AddViewData(PageName);
                return View(studentOutput);
            }
            catch
            {
                return View(viewName: "_Error");
            }
        }

        [Route("create")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync();
            ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");
            AddViewData(PageName);
            return View();
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(StudentInput studentInput)
        {
            try
            {
                List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync();
                ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");
                
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
                List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync();
                ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");
                StudentOutput studentOutput = await _studentRepository.GetAsync(id);
                if (studentOutput == null)
                    return RedirectToAction("Index");

                AddViewData(PageName);
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
                List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync();
                ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");
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

        [Route("delete/{id}")]
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
