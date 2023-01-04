using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IFacultyStaffRepository : ISubRepository<FacultyStaffInput, FacultyStaffOutput, string>, IAccountPattern
{

}
