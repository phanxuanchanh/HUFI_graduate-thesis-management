using GraduateThesis.Models;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using GraduateThesis.RepositoryPatterns;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces
{
    public interface IThesisRepository : ICrudPattern<Thesis, ThesisInput, ThesisOutput, string>, IRepositoryConfiguration
    {
        Task<DataResponse> DoThesisRegisterAsync(ThesisRegisterInput thesisRegisterInput);
    }
}
