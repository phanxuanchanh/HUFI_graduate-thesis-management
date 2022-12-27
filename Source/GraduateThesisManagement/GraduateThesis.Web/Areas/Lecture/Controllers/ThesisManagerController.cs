using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using GraduateThesis.Common.WebAttributes;
using NPOI.SS.UserModel;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Net;
using GraduateThesis.Common.File;
using NPOI.HPSF;
using System.Net.Mime;
using GraduateThesis.Repository.BLL.Implements;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/thesis-manager")]
    public class ThesisManagerController : WebControllerBase
    {
        private ITopicRepository _topicRepository;
        private IStudentThesisGroupRepository _studentThesisGroupRepository;
        private ITrainingFormRepository _trainingFormRepository;
        private IFacultyStaffRepository _facultyStaffRepository;
        private ITrainingLevelRepository _trainingLevelRepository;
        private IThesisRepository _thesisRepository;
        private ISpecializationRepository _specializationRepository;

        public ThesisManagerController(IRepository repository)
        {
            _thesisRepository = repository.ThesisRepository;
            _studentThesisGroupRepository = repository.StudentThesisGroupRepository;
            _trainingFormRepository = repository.TrainingFormRepository;
            _trainingLevelRepository = repository.TrainingLevelRepository;
            _facultyStaffRepository = repository.FacultyStaffRepository;
            _topicRepository = repository.TopicRepository;
            _specializationRepository = repository.SpecializationRepository;

        }

        [Route("list")]
        [HttpGet]
        [PageName(Name = "Danh sách các đề tài khóa luận")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
        {
            try
            {
                
                List<ThesisOutput> thesesName = await _thesisRepository.GetListAsync();
                ViewData["thesesName"] = new SelectList(thesesName, "Id", "Name");
                List<ThesisOutput> thesesMaxStudentNumber = await _thesisRepository.GetListAsync();
                ViewData["thesesMaxStudentNumber"] = new SelectList(thesesName, "Id", "MaxStudentNumber");
                List<ThesisOutput> thesesSemester = await _thesisRepository.GetListAsync();
                ViewData["thesesSemester"] = new SelectList(thesesName, "Id", "Semester");  
                Pagination<ThesisOutput> pagination;
                if (orderOptions == "ASC")
                    pagination = await _thesisRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);
                else
                    pagination = await _thesisRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.DESC, keyword);

                StaticPagedList<ThesisOutput> pagedList = pagination.ToStaticPagedList();
                ViewData["PagedList"] = pagedList;
                ViewData["OrderBy"] = orderBy;
                ViewData["OrderOptions"] = orderOptions;
                ViewData["Keyword"] = keyword;

                return View();
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("details/{id}")]
        [HttpGet]
        [PageName(Name = "Chi tiết đề tài khóa luận")]
        public async Task<IActionResult> Details([Required] string id)
        {
            try
            {
                ThesisOutput thesisOutput = await _thesisRepository.GetAsync(id);
                if (thesisOutput == null)
                    return RedirectToAction("Index");

                return View(thesisOutput);
            }
            catch
            {
                return View(viewName: "_Error");
            }
        }

        [Route("create")]
        [HttpGet]
        [PageName(Name = "Tạo mới đề tài khóa luận")]
        public async Task<ActionResult> Create()
        {
            List<TopicOutput> topicClasses = await _topicRepository.GetListAsync();
            ViewData["TopicList"] = new SelectList(topicClasses, "Id", "Name");

            List<StudentThesisGroupOutput> StudentThesisGrouClasses = await _studentThesisGroupRepository.GetListAsync();
            ViewData["StudentThesisGrouList"] = new SelectList(StudentThesisGrouClasses, "Id", "Name");

            List<TrainingFormOutput> trainingFormsClass = await _trainingFormRepository.GetListAsync();
            ViewData["TrainingFormList"] = new SelectList(trainingFormsClass, "Id", "Name");

            List<TrainingLevelOutput> trainingLevelsClass = await _trainingLevelRepository.GetListAsync();
            ViewData["TrainingLevelList"] = new SelectList(trainingLevelsClass, "Id", "Name");

            List<FacultyStaffOutput> facultyStaffClass = await _facultyStaffRepository.GetListAsync();
            ViewData["FacultyStaffList"] = new SelectList(facultyStaffClass, "Id", "FullName");

            List<SpecializationOutput> specializationsClass = await _specializationRepository.GetListAsync();
            ViewData["SpecializationsClass"] = new SelectList(specializationsClass, "Id", "Name");

            return View();
        }

        [Route("create")]
        [HttpPost]
        [PageName(Name = "Tạo mới đề tài khóa luận")]
        public async Task<IActionResult> Create(ThesisInput thesisInput)
        {
            try
            {
                List<TopicOutput> topicClasses = await _topicRepository.GetListAsync();
                ViewData["TopicList"] = new SelectList(topicClasses, "Id", "Name");

                List<StudentThesisGroupOutput> StudentThesisGrouClasses = await _studentThesisGroupRepository.GetListAsync();
                ViewData["StudentThesisGrouList"] = new SelectList(StudentThesisGrouClasses, "Id", "Name");

                List<TrainingFormOutput> trainingFormsClass = await _trainingFormRepository.GetListAsync();
                ViewData["TrainingFormList"] = new SelectList(trainingFormsClass, "Id", "Name");

                List<TrainingLevelOutput> trainingLevelsClass = await _trainingLevelRepository.GetListAsync();
                ViewData["TrainingLevelList"] = new SelectList(trainingLevelsClass, "Id", "Name");

                List<FacultyStaffOutput> facultyStaffClass = await _facultyStaffRepository.GetListAsync();
                ViewData["FacultyStaffList"] = new SelectList(facultyStaffClass, "Id", "FullName");

                List<SpecializationOutput> specializationsClass = await _specializationRepository.GetListAsync();
                ViewData["SpecializationsClass"] = new SelectList(specializationsClass, "Id", "Name");

                if (ModelState.IsValid)
                {
                    DataResponse<ThesisOutput> dataResponse = await _thesisRepository.CreateAsync(thesisInput);
                    AddViewData(dataResponse);

                    return View(thesisInput);
                }

                AddViewData(DataResponseStatus.InvalidData);
                return View(thesisInput);
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("edit/{id}")]
        [HttpGet]
        [PageName(Name = "Chỉnh sửa đề tài khóa luận")]
        public async Task<IActionResult> Edit([Required] string id)
        {
            try
            {
                List<TopicOutput> topicClasses = await _topicRepository.GetListAsync();
                ViewData["TopicList"] = new SelectList(topicClasses, "Id", "Name");

                List<StudentThesisGroupOutput> StudentThesisGrouClasses = await _studentThesisGroupRepository.GetListAsync();
                ViewData["StudentThesisGrouList"] = new SelectList(StudentThesisGrouClasses, "Id", "Name");

                List<TrainingFormOutput> trainingFormsClass = await _trainingFormRepository.GetListAsync();
                ViewData["TrainingFormList"] = new SelectList(trainingFormsClass, "Id", "Name");

                List<TrainingLevelOutput> trainingLevelsClass = await _trainingLevelRepository.GetListAsync();
                ViewData["TrainingLevelList"] = new SelectList(trainingLevelsClass, "Id", "Name");

                List<FacultyStaffOutput> facultyStaffClass = await _facultyStaffRepository.GetListAsync();
                ViewData["FacultyStaffList"] = new SelectList(facultyStaffClass, "Id", "FullName");

                List<SpecializationOutput> specializationsClass = await _specializationRepository.GetListAsync();
                ViewData["SpecializationsClass"] = new SelectList(specializationsClass, "Id", "Name");

                ThesisOutput thesisOutput = await _thesisRepository.GetAsync(id);
                if (thesisOutput == null)
                    return RedirectToAction("Index");

                return View(thesisOutput);
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("edit/{id}")]
        [HttpPost]
        [PageName(Name = "Chỉnh sửa đề tài khóa luận")]
        public async Task<IActionResult> Edit([Required] string id, ThesisInput thesisInput)
        {
            try
            {
                List<TopicOutput> topicClasses = await _topicRepository.GetListAsync();
                ViewData["TopicList"] = new SelectList(topicClasses, "Id", "Name");

                List<StudentThesisGroupOutput> StudentThesisGrouClasses = await _studentThesisGroupRepository.GetListAsync();
                ViewData["StudentThesisGrouList"] = new SelectList(StudentThesisGrouClasses, "Id", "Name");

                List<TrainingFormOutput> trainingFormsClass = await _trainingFormRepository.GetListAsync();
                ViewData["TrainingFormList"] = new SelectList(trainingFormsClass, "Id", "Name");

                List<TrainingLevelOutput> trainingLevelsClass = await _trainingLevelRepository.GetListAsync();
                ViewData["TrainingLevelList"] = new SelectList(trainingLevelsClass, "Id", "Name");

                List<FacultyStaffOutput> facultyStaffClass = await _facultyStaffRepository.GetListAsync();
                ViewData["FacultyStaffList"] = new SelectList(facultyStaffClass, "Id", "FullName");

                List<SpecializationOutput> specializationsClass = await _specializationRepository.GetListAsync();
                ViewData["SpecializationsClass"] = new SelectList(specializationsClass, "Id", "Name");

                if (ModelState.IsValid)
                {
                    ThesisOutput thesisOutput = await _thesisRepository.GetAsync(id);
                    if (thesisOutput == null)
                        return RedirectToAction("Index");

                    DataResponse<ThesisOutput> dataResponse = await _thesisRepository.UpdateAsync(id, thesisInput);

                    AddViewData(dataResponse);
                    return View(thesisInput);
                }

                AddViewData(DataResponseStatus.InvalidData);
                return View(thesisInput);
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
                ThesisOutput thesisOutput = await _thesisRepository.GetAsync(id);
                if (thesisOutput == null)
                    return RedirectToAction("Index");

                DataResponse dataResponse = await _thesisRepository.BatchDeleteAsync(id);

                AddTempData(dataResponse);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("export-to-spreadsheet")]
        [HttpGet]
        public async Task<IActionResult> ExportToSpreadsheet()
        {
            try
            {
                IWorkbook workbook = await _thesisRepository.ExportToSpreadsheetAsync(
                    SpreadsheetTypeOptions.XLSX, 
                    "Danh sách đề tài",
                    new string[] { "Id", "Name", "Description", "MaxStudentNumber", "Semester" }
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
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("import")]
        [HttpGet]
        [PageName(Name = "Import")]
        public async Task<IActionResult> Import()
        {
                return View();
        
        }
    }
}
