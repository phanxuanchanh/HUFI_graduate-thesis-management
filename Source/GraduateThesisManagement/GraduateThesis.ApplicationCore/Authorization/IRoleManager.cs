
namespace GraduateThesis.ApplicationCore.Authorization;

public interface IRoleManager
{
    Task<bool> IsValidAsync(string userId, string controllerName, string actionName);
}
