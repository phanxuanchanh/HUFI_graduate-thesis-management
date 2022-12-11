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
        IThesisCommitteeRepository ThesisCommitteeRepository { get; }
        ICommitteeMemberRepository CommitteeMemberRepository { get; }
        IStudentThesisGroupRepository StudentThesisGroupRepository { get; }
        IFacultyRepository FacultyRepository { get; }
    }
}
