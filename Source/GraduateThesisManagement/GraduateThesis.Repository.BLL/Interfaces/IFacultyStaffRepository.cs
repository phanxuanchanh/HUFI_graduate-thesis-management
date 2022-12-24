using GraduateThesis.Models;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using GraduateThesis.RepositoryPatterns;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces
{
    public interface IFacultyStaffRepository : ICrudPattern<FacultyStaff, FacultyStaffInput, FacultyStaffOutput, string>, IAccountPattern, IRepositoryConfiguration
    {
        Task<List<FacultyStaffOutput>> GetListByRoleIdAsync(string roleId, int count = 200);
        Task<Pagination<FacultyStaffOutput>> GetPaginationByRoleIdAsync(string roleId, int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword);
    }
}
