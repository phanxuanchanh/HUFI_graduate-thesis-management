using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces
{
    public interface IRepository : IDisposable
    {
        IStudentRepository StudentRepository { get; }
        IStudentClassRepository StudentClassRepository { get; }
        IThesisRepository ThesisRepository { get; }
        ITopicRepository TopicRepository { get; }
        ICouncilRepository CouncilRepository { get; }
        ICouncilMemberRepository CouncilMemberRepository { get; }
        IStudentThesisGroupRepository StudentThesisGroupRepository { get; }
        IFacultyRepository FacultyRepository { get; }
        ILecturerRepository LecturerRepository { get; }

    }
}
