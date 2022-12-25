using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/facutlystaff-manager")]
    public class FacultyStaffManagerController : WebControllerBase
    {
        public string PageName { get; set; } = "Quản lý giảng viên";

        private IFacultyStaffRepository _facultyStaffRepository;
        private IFacultyRepository _facultyRepository;
        private IFacultyStaffRoleRepository _facultyStaffRoleRepository;



        public FacultyStaffManagerController(IRepository repository)
        {
            _facultyStaffRepository = repository.FacultyStaffRepository;
            _facultyRepository = repository.FacultyRepository;
            _facultyStaffRoleRepository= repository.FacultyStaffRoleRepository;

        }

        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = null, string orderOptions = "ASC", string keyword = null)
        {
            try
            {
                Pagination<FacultyStaffOutput> pagination;
                if (orderOptions == "ASC")
                    pagination = await _facultyStaffRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);
                else
                    pagination = await _facultyStaffRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.DESC, keyword);

                StaticPagedList<FacultyStaffOutput> pagedList = pagination.ToStaticPagedList();
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
                FacultyStaffOutput facultyStaffOutput = await _facultyStaffRepository.GetAsync(id);
                if (facultyStaffOutput == null)
                    return RedirectToAction("Index");

                AddViewData(PageName);
                return View(facultyStaffOutput);
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
            List<FacultyOutput> faculties = await _facultyRepository.GetListAsync();
            ViewData["FacultyList"] = new SelectList(faculties, "Id", "Name");

            List<FacultyStaffRoleOutput> facultyStaffRoles = await _facultyStaffRoleRepository.GetListAsync();
            ViewData["facultyStaffRolesList"] = new SelectList(facultyStaffRoles, "Id", "Name");

            AddViewData(PageName);
            return View();
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(FacultyStaffInput facultyStaffInput)
        {
            try
            {
                List<FacultyOutput> faculties = await _facultyRepository.GetListAsync();
                ViewData["FacultyList"] = new SelectList(faculties, "Id", "Name");

                List<FacultyStaffRoleOutput> facultyStaffRoles = await _facultyStaffRoleRepository.GetListAsync();
                ViewData["facultyStaffRolesList"] = new SelectList(facultyStaffRoles, "Id", "Name");

                if (ModelState.IsValid)
                {
                    DataResponse<FacultyStaffOutput> dataResponse = await _facultyStaffRepository.CreateAsync(facultyStaffInput);
                    AddViewData(PageName, dataResponse);

                    return View(facultyStaffInput);
                }

                AddViewData(PageName, DataResponseStatus.InvalidData);
                return View(facultyStaffInput);
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
                List<FacultyOutput> faculties = await _facultyRepository.GetListAsync();
                ViewData["FacultyList"] = new SelectList(faculties, "Id", "Name");

                List<FacultyStaffRoleOutput> facultyStaffRoles = await _facultyStaffRoleRepository.GetListAsync();
                ViewData["facultyStaffRolesList"] = new SelectList(facultyStaffRoles, "Id", "Name");

                FacultyStaffOutput facultyStaffOutput = await _facultyStaffRepository.GetAsync(id);
                if (facultyStaffOutput == null)
                    return RedirectToAction("Index");

                AddViewData(PageName);
                return View(facultyStaffOutput);
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit([Required] string id, FacultyStaffInput facultyStaffInput)
        {
            try
            {
                List<FacultyOutput> faculties = await _facultyRepository.GetListAsync();
                ViewData["FacultyList"] = new SelectList(faculties, "Id", "Name");

                List<FacultyStaffRoleOutput> facultyStaffRoles = await _facultyStaffRoleRepository.GetListAsync();
                ViewData["facultyStaffRolesList"] = new SelectList(facultyStaffRoles, "Id", "Name");
                if (ModelState.IsValid)
                {
                    FacultyStaffOutput facultyStaffOutput = await _facultyStaffRepository.GetAsync(id);
                    if (facultyStaffOutput == null)
                        return RedirectToAction("Index");

                    DataResponse<FacultyStaffOutput> dataResponse = await _facultyStaffRepository.UpdateAsync(id, facultyStaffInput);

                    AddViewData(PageName, dataResponse);
                    return View(facultyStaffInput);
                }

                AddViewData(PageName, DataResponseStatus.InvalidData);
                return View(facultyStaffInput);
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
                FacultyStaffOutput facultyStaffOutput = await _facultyStaffRepository.GetAsync(id);
                if (facultyStaffOutput == null)
                    return RedirectToAction("Index");

                DataResponse dataResponse = await _facultyStaffRepository.BatchDeleteAsync(id);

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
