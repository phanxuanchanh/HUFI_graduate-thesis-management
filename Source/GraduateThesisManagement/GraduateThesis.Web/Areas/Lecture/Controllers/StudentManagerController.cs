using GraduateThesis.Common.Authorization;
using GraduateThesis.Common.File;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/student-manager")]
    [WebAuthorize(AccountRole.Lecture)]
    [AccountInfo(typeof(FacultyStaffOutput))]
    [HandleException]
    public class StudentManagerController : WebControllerBase
    {
        private IStudentRepository _studentRepository;
        private IStudentClassRepository _studentClassRepository;

        public StudentManagerController(IRepository repository)
        {
            _studentRepository = repository.StudentRepository;
            _studentClassRepository = repository.StudentClassRepository;
        }

        [Route("list")]
        [HttpGet]
        [PageName(Name = "Danh sách sinh viên của khoa")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
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

            return View();
        }

        [Route("details/{id}")]
        [HttpGet]
        [PageName(Name = "Chi tiết sinh viên của khoa")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Details([Required] string id)
        {
            StudentOutput studentOutput = await _studentRepository.GetAsync(id);
            if (studentOutput == null)
                return RedirectToAction("Index");

            return View(studentOutput);
        }

        [Route("create")]
        [HttpGet]
        [PageName(Name = "Tạo mới sinh viên của khoa")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<ActionResult> Create()
        {
            List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync();
            ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");
            return View();
        }

        [Route("create")]
        [HttpPost]
        [PageName(Name = "Tạo mới sinh viên của khoa")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Create(StudentInput studentInput)
        {
            List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync();
            ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");

            if (ModelState.IsValid)
            {
                DataResponse<StudentOutput> dataResponse = await _studentRepository.CreateAsync(studentInput);
                AddViewData(dataResponse);

                return View(studentInput);
            }

            AddViewData(DataResponseStatus.InvalidData);
            return View(studentInput);
        }

        [Route("edit/{id}")]
        [HttpGet]
        [PageName(Name = "Chỉnh sửa thông tin sinh viên của khoa")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Edit([Required] string id)
        {
            List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync();
            ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");
            StudentOutput studentOutput = await _studentRepository.GetAsync(id);
            if (studentOutput == null)
                return RedirectToAction("Index");

            return View(studentOutput);
        }

        [Route("edit/{id}")]
        [HttpPost]
        [PageName(Name = "Chỉnh sửa thông tin sinh viên của khoa")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Edit([Required] string id, StudentInput studentInput)
        {
            List<StudentClassOutput> studentClasses = await _studentClassRepository.GetListAsync();
            ViewData["StudentClassList"] = new SelectList(studentClasses, "Id", "Name");
            if (ModelState.IsValid)
            {
                StudentOutput studentOutput = await _studentRepository.GetAsync(id);
                if (studentOutput == null)
                    return RedirectToAction("Index");

                DataResponse<StudentOutput> dataResponse = await _studentRepository.UpdateAsync(id, studentInput);

                AddViewData(dataResponse);
                return View(studentInput);
            }

            AddViewData(DataResponseStatus.InvalidData);
            return View(studentInput);
        }

        [Route("delete/{id}")]
        [HttpPost]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Delete([Required] string id)
        {
            StudentOutput studentOutput = await _studentRepository.GetAsync(id);
            if (studentOutput == null)
                return RedirectToAction("Index");

            DataResponse dataResponse = await _studentRepository.BatchDeleteAsync(id);

            AddTempData(dataResponse);
            return RedirectToAction("Index");
        }

        [Route("export-to-spreadsheet")]
        [HttpGet]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> ExportToSpreadsheet()
        {
            IWorkbook workbook = await _studentRepository.ExportToSpreadsheetAsync(
                SpreadsheetTypeOptions.XLSX,
                "Danh sách sinh viên của khoa",
                new string[] { "Id", "Name", "Phone", "EMail", "Address", "Birthday", "Avatar", "Description", "StudentClassId" }
            );

            ContentDisposition contentDisposition = new ContentDisposition
            {
                FileName = $"Thesis_{DateTime.Now.ToString("ddMMyyyy_hhmmss")}.xlsx",
                Inline = true,
            };

            Response.Headers.Append("Content-Disposition", contentDisposition.ToString());

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.Write(memoryStream, true);
                byte[] bytes = memoryStream.ToArray();
                return File(bytes, ContentTypeConsts.XLSX);
            }
        }
        [Route("import")]

        public IActionResult Import()
        {
            return View();
        }
    }
}
