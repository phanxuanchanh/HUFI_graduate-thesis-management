using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IThesisRepository : ISubRepository<ThesisInput, ThesisOutput, string>
{
    Task<DataResponse> DoThesisRegisterAsync(ThesisRegisterInput thesisRegisterInput);
}
