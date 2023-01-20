using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/thesisrevision-manager")]
    public class ThesisRevisionManager : WebControllerBase<IThesisRevisionRepository, ThesisRevisionInput, ThesisRevisionOutput, string>
    {
        private IThesisRevisionRepository _thesisRevisionRepository;
        private IThesisRepository _thesisRepository;

        public ThesisRevisionManager(IRepository repository)
            : base(repository.ThesisRevisionRepository)
        {
            _thesisRevisionRepository = repository.ThesisRevisionRepository;
            _thesisRepository = repository.ThesisRepository;

        }
        private Func<Task> LoadSelectList()
        {
            Func<Task> dependency = async () =>
            {
                List<ThesisOutput> thesisOutputs = await _thesisRepository.GetListAsync(50);
                ViewData["ThesisList"] = new SelectList(thesisOutputs, "Id", "Name");

            };

            return dependency;
        }
        public override Task<IActionResult> BatchDelete([Required] string id)
        {
            throw new NotImplementedException();
        }

        [Route("create")]
        [HttpGet]
        [PageName(Name = "Tạo mới quản lý kết quả thực hiện đề tài đồ án")]
        public override async Task<IActionResult> Create()
        {
            return await CreateResult();
        }

        [Route("create")]
        [HttpPost]
        [PageName(Name = "Tạo mới quản lý kết quả thực hiện đề tài đồ án")]
        public override async Task<IActionResult> Create(ThesisRevisionInput thesisRevisionInput)
        {
            return await CreateResult(thesisRevisionInput);
        }

        [Route("details/{id}")]
        [HttpGet]
        [PageName(Name = "Chi tiết quản lý kết quả thực hiện đề tài đồ án")]
        public override async Task<IActionResult> Details([Required] string id)
        {
            return await GetDetailsResult(id);
        }


        public override Task<IActionResult> Edit([Required] string id)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> Edit([Required] string id, ThesisRevisionInput input)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> Export()
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> ForceDelete([Required] string id)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> GetTrash(int count = 50)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> Import()
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> Import([FromForm] IFormFile formFile, ImportMetadata importMetadata)
        {
            throw new NotImplementedException();
        }

      
        [Route("list")]
        [HttpGet]
        [PageName(Name = "Danh sách quản lý kết quả thực hiện đề tài đồ án ")]
        public override async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
        {
            return await IndexResult(page, pageSize, orderBy, orderOptions, keyword);
        }

        public override Task<IActionResult> Restore([Required] string id)
        {
            throw new NotImplementedException();
        }
    }
}
