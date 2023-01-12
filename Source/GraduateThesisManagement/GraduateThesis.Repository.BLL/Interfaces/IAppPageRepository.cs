using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IAppPageRepository : ISubRepository<AppPageInput, AppPageOutput, string>
{
    Task<DataResponse> GrantAsync(AppRoleMappingInput input);
    Task<DataResponse> RevokeAsync(AppRoleMappingInput input);
}
