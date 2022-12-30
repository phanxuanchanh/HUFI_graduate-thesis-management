using GraduateThesis.Common.Authorization;
using GraduateThesis.Common.File;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NuGet.Protocol.Core.Types;
using System.Net.Mime;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/thesisgroup-manager")]
    [WebAuthorize(AccountRole.Lecture)]
    [AccountInfo(typeof(FacultyStaffOutput))]
    [HandleException]
    public class ThesisGroupManagerController : WebControllerBase
    {
        private IStudentThesisGroupRepository _studentThesisGroupRepository;
   
        public ThesisGroupManagerController(IRepository repository)
        {
            _studentThesisGroupRepository = repository.StudentThesisGroupRepository;
           
        }

        [Route("list")]
        [HttpGet]
        [PageName(Name = "Danh sách nhóm sinh viên làm khóa luận")]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
        {
                Pagination<StudentThesisGroupOutput> pagination;
                if (orderOptions == "ASC")
                    pagination = await _studentThesisGroupRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);
                else
                    pagination = await _studentThesisGroupRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.DESC, keyword);

                StaticPagedList<StudentThesisGroupOutput> pagedList = pagination.ToStaticPagedList();
                ViewData["PagedList"] = pagedList;
                ViewData["OrderBy"] = orderBy;
                ViewData["OrderOptions"] = orderOptions;
                ViewData["Keyword"] = keyword;

                return View();          
        }
        [Route("export-to-spreadsheet")]
        [HttpGet]
        [WebAuthorize(AccountRole.Lecture)]
        public async Task<IActionResult> ExportToSpreadsheet()
        {
            IWorkbook workbook = await _studentThesisGroupRepository.ExportToSpreadsheetAsync(
                SpreadsheetTypeOptions.XLSX,
                "Danh sách nhóm sinh viên làm khóa luận ",
                new string[] { "Id", "Name" }
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
