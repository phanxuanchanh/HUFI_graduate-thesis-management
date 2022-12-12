using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using GraduateThesis.RepositoryPatterns;

namespace GraduateThesis.Repository.BLL.Interfaces
{
    public interface IFacultyStaffRepository : ICrudPattern<FacultyStaff, FacultyStaffInput, FacultyStaffOutput, string>, IAccountPattern, IRepositoryConfiguration
    {

    }
}
