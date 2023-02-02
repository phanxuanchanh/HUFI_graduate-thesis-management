using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IAppRoleRepository : IAsyncSubRepository<AppRoleInput, AppRoleOutput, string>
{
    Task<DataResponse> GrantToPageAsync(AppRoleMappingInput input);
    Task<DataResponse> RevokeFromPageAsync(AppRoleMappingInput input);
}
