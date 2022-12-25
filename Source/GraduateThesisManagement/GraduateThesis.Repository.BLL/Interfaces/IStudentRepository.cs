using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using GraduateThesis.RepositoryPatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces
{
    public interface IStudentRepository : ICrudPattern<Student, StudentInput, StudentOutput, string>, IAccountPattern, IRepositoryConfiguration
    {
        Task<StudentThesisOutput> GetStudentThesisAsync(string studentId);
    }
}
