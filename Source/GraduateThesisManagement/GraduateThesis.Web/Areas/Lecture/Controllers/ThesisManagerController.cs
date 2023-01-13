using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.AppController;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/thesis-manager")]
    [WebAuthorize]
    [AccountInfo(typeof(FacultyStaffOutput))]
    public class ThesisManagerController : WebControllerBase<IThesisRepository, ThesisInput, ThesisOutput, string>
    {
        private ITopicRepository _topicRepository;
        private IThesisGroupRepository _studentThesisGroupRepository;
        private ITrainingFormRepository _trainingFormRepository;
        private IFacultyStaffRepository _facultyStaffRepository;
        private ITrainingLevelRepository _trainingLevelRepository;
        private IThesisRepository _thesisRepository;
        private ISpecializationRepository _specializationRepository;

        public ThesisManagerController(IRepository repository)
            : base(repository.ThesisRepository)
        {
            _thesisRepository = repository.ThesisRepository;
            _studentThesisGroupRepository = repository.ThesisGroupRepository;
            _trainingFormRepository = repository.TrainingFormRepository;
            _trainingLevelRepository = repository.TrainingLevelRepository;
            _facultyStaffRepository = repository.FacultyStaffRepository;
            _topicRepository = repository.TopicRepository;
            _specializationRepository = repository.SpecializationRepository;
        }

        private Func<Task> LoadSelectList()
        {
            Func<Task> dependency = async () =>
            {
                List<TopicOutput> topicClasses = await _topicRepository.GetListAsync(50);
                ViewData["TopicList"] = new SelectList(topicClasses, "Id", "Name");

                List<ThesisGroupOutput> thesisGrouClasses = await _studentThesisGroupRepository.GetListAsync(50);
                ViewData["StudentThesisGrouList"] = new SelectList(thesisGrouClasses, "Id", "Name");

                List<TrainingFormOutput> trainingFormsClass = await _trainingFormRepository.GetListAsync(50);
                ViewData["TrainingFormList"] = new SelectList(trainingFormsClass, "Id", "Name");

                List<TrainingLevelOutput> trainingLevelsClass = await _trainingLevelRepository.GetListAsync(50);
                ViewData["TrainingLevelList"] = new SelectList(trainingLevelsClass, "Id", "Name");

                List<FacultyStaffOutput> facultyStaffClass = await _facultyStaffRepository.GetListAsync(50);
                ViewData["FacultyStaffList"] = new SelectList(facultyStaffClass, "Id", "FullName");

                List<SpecializationOutput> specializationsClass = await _specializationRepository.GetListAsync(50);
                ViewData["SpecializationsClass"] = new SelectList(specializationsClass, "Id", "Name");
            };

            return dependency;
        }

        [Route("list")]
        [HttpGet]
        [PageName(Name = "Danh sách các đề tài khóa luận")]
        public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
        {
            return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
        }

        [Route("details/{id}")]
        [HttpGet]
        [PageName(Name = "Chi tiết đề tài khóa luận")]
        public override async Task<IActionResult> Details([Required] string id)
        {
            return await GetDetailsResult(id);
        }

        [Route("create")]
        [HttpGet]
        [PageName(Name = "Tạo mới đề tài khóa luận")]
        public override async Task<IActionResult> Create()
        {
            return await CreateResult(LoadSelectList());
        }

        [Route("create")]
        [HttpPost]
        [PageName(Name = "Tạo mới đề tài khóa luận")]
        public override async Task<IActionResult> Create(ThesisInput thesisInput)
        {
            return await CreateResult(thesisInput, LoadSelectList());
        }

        [Route("edit/{id}")]
        [HttpGet]
        [PageName(Name = "Chỉnh sửa đề tài khóa luận")]
        public override async Task<IActionResult> Edit([Required] string id)
        {
            return await EditResult(id, LoadSelectList());
        }

        [Route("edit/{id}")]
        [HttpPost]
        [PageName(Name = "Chỉnh sửa đề tài khóa luận")]
        public override async Task<IActionResult> Edit([Required] string id, ThesisInput thesisInput)
        {
            return await EditResult(id, thesisInput);
        }

        [Route("delete/{id}")]
        [HttpPost]
        public override async Task<IActionResult> BatchDelete([Required] string id)
        {
            return await BatchDeleteResult(id);
        }

        public override async Task<IActionResult> ForceDelete([Required] string id)
        {
            return await ForceDeleteResult(id);
        }

        public override async Task<IActionResult> Export()
        {
            return await ExportResult(null!, null!);
        }

        public override async Task<IActionResult> Import(IFormFile formFile)
        {
            return await ImportResult(formFile, new ImportMetadata());
        }

        public override Task<IActionResult> Import()
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> GetTrash(int count = 50)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> Restore([Required] string id)
        {
            throw new NotImplementedException();
        }
    }
}



