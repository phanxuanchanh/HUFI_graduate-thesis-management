using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IFacultyStaffRepository : ISubRepository<FacultyStaffInput, FacultyStaffOutput, string>, IAsyncAccountPattern
{
    IEmailService EmailService { get; set; }

    Task<Pagination<FacultyStaffOutput>> GetPgnHasRoleIdAsync(string roleId, int page, int pageSize, string keyword);
}
