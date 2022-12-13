using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using X.PagedList;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/thesis-manager")]
    public class ThesisManagerController : WebControllerBase
    {
        public string PageName { get; set; } = "Quản lý đề tài";

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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = null, string orderOptions = "ASC", string keyword = null)
        {
            try
            {
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

                AddViewData(PageName);

                return View();
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }

        [Route("create")]
        [HttpGet]
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
            ViewData["FacultyStaffList"] = new SelectList(specializationsClass, "Id", "FullName");

            AddViewData(PageName);
            return View();
        }

        [Route("create")]
        [HttpPost]
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
                ViewData["FacultyStaffList"] = new SelectList(specializationsClass, "Id", "FullName");

                if (ModelState.IsValid)
                {
                    DataResponse<ThesisOutput> dataResponse = await _thesisRepository.CreateAsync(thesisInput);
                    AddViewData(PageName, dataResponse);

                    return View(thesisInput);
                }

                AddViewData(PageName, DataResponseStatus.InvalidData);
                return View(thesisInput);
            }
            catch (Exception ex)
            {
                return View(viewName: "_Error", model: ex.Message);
            }
        }
    }
}
