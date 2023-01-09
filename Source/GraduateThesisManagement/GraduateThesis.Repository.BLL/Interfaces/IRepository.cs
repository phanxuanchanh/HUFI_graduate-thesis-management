using System;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface IRepository : IDisposable
{
    IStudentRepository StudentRepository { get; }
    IStudentClassRepository StudentClassRepository { get; }
    IThesisRepository ThesisRepository { get; }
    ITopicRepository TopicRepository { get; }
    IThesisCommitteeRepository ThesisCommitteeRepository { get; }
    ICommitteeMemberRepository CommitteeMemberRepository { get; }
    IThesisGroupRepository StudentThesisGroupRepository { get; }
    IFacultyRepository FacultyRepository { get; }
    IFacultyStaffRepository FacultyStaffRepository { get; }
    ITrainingFormRepository TrainingFormRepository { get; }
    ITrainingLevelRepository TrainingLevelRepository { get; }
    ISpecializationRepository SpecializationRepository { get; }
    IAppRoleRepository AppRolesRepository { get; }
    IThesisRevisionRepository ThesisRevisionRepository { get; }

    int ExecuteNonQuery(FormattableString sql);
    T ExecuteScalar<T>(FormattableString sql);
}
