using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IAppRoleRepository : ISubRepository<AppRoleInput, AppRoleOutput, string>
{
    Task<DataResponse> GrantAsync(AppUserRoleInput input);
    Task<DataResponse> RevokeAsync(AppUserRoleInput input);
}
