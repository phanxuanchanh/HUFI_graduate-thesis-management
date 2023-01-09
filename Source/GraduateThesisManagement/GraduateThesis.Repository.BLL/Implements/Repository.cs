using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GraduateThesis.Repository.BLL.Implements;

public class Repository : IRepository
{
    private HufiGraduateThesisContext _context;

    public Repository(Action<DbContextOptionsBuilder> optionsAction)
    {
        DbContextOptionsBuilder<HufiGraduateThesisContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<HufiGraduateThesisContext>();
        optionsAction.Invoke(dbContextOptionsBuilder);
        _context = new HufiGraduateThesisContext(dbContextOptionsBuilder.Options);
    }

    private bool disposedValue;

    public IStudentRepository StudentRepository => new StudentRepository(_context);

    public IStudentClassRepository StudentClassRepository => new StudentClassRepository(_context);

    public IThesisRepository ThesisRepository => new ThesisRepository(_context);

    public ITopicRepository TopicRepository => new TopicRepository(_context);

    public IThesisGroupRepository ThesisGroupRepository => new ThesisGroupRepository(_context);

    public IFacultyRepository FacultyRepository => new FacultyRepository(_context);

    public ICommitteeMemberRepository CommitteeMemberRepository => new CommitteeMemberRepository(_context);

    public IThesisCommitteeRepository ThesisCommitteeRepository => new ThesisCommitteeRepository(_context);

    public IFacultyStaffRepository FacultyStaffRepository => new FacultyStaffRepository(_context);

    public ITrainingFormRepository TrainingFormRepository => new TrainingFormRepository(_context);

    public ITrainingLevelRepository TrainingLevelRepository => new TrainingLevelRepository(_context);

    public ISpecializationRepository SpecializationRepository => new SpecializationRepository(_context);

    public IAppRoleRepository AppRolesRepository => new AppRoleRepository(_context);

    public IThesisRevisionRepository ThesisRevisionRepository => new ThesisRevisionRepository(_context);
    
    
    public int ExecuteNonQuery(FormattableString sql)
    {
        return _context.Database.ExecuteSql(sql);
    }

    public T ExecuteScalar<T>(FormattableString sql)
    {
        return _context.Database.SqlQuery<T>(sql).SingleOrDefault();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {

            }

            this._context.Dispose();

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
