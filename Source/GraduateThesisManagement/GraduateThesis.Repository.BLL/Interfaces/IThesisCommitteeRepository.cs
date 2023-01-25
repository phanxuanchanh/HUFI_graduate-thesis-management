using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisCommitteeRepository : IAsyncSubRepository<ThesisCommitteeInput, ThesisCommitteeOutput, string>
{
}
