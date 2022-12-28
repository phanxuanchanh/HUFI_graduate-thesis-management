using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Implements;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace GraduateThesis.Web.Areas.Lecture.Controllers
{
    [Area("Lecture")]
    [Route("lecture/appUserRole-manager")]
    [HandleException]
    public class AppUserRoleManagerController : WebControllerBase
    {
        private IAppRolesRepository _appRolesRepository;
        private IFacultyRepository _facultyRepository;
        private IAppUserRolesRepository _appUserRolesRepository;

        public AppUserRoleManagerController(IRepository repository)
        {
            _appRolesRepository = repository.AppRolesRepository;
            _facultyRepository = repository.FacultyRepository;
            _appUserRolesRepository = repository.AppUserRolesRepository;
          
        }

        [Route("list")]
        [HttpGet]
        [PageName(Name = "Danh sách giảng viên và chức vụ")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string orderBy = "", string orderOptions = "ASC", string keyword = "")
        {
           
                Pagination<AppUserRoleOutput> pagination;
                if (orderOptions == "ASC")
                    pagination = await _appUserRolesRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.ASC, keyword);
                else
                    pagination = await _appUserRolesRepository.GetPaginationAsync(page, pageSize, orderBy, OrderOptions.DESC, keyword);

                StaticPagedList<AppUserRoleOutput> pagedList = pagination.ToStaticPagedList();
                ViewData["PagedList"] = pagedList;
                ViewData["OrderBy"] = orderBy;
                ViewData["OrderOptions"] = orderOptions;
                ViewData["Keyword"] = keyword;

                return View();
            
           
        }

    }
}
