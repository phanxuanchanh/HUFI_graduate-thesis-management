﻿using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System;

namespace GraduateThesis.Repository.BLL.Implements
{
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

        public IStudentThesisGroupRepository StudentThesisGroupRepository => new StudentThesisGroupRepository(_context);

        public IFacultyRepository FacultyRepository  => new FacultyRepository(_context);

        public ICommitteeMemberRepository CommitteeMemberRepository => new CommitteeMemberRepository(_context);

        public IThesisCommitteeRepository ThesisCommitteeRepository => new ThesisCommitteeRepository(_context);

        public IFacultyStaffRepository FacultyStaffRepository => new FacultyStaffRepository(_context);

        public ITrainingFormRepository TrainingFormRepository => new TrainingFormRepository(_context);

        public ITrainingLevelRepository TrainingLevelRepository => new TrainingLevelRepository(_context);

        public ISpecializationRepository SpecializationRepository => new SpecializationRepository(_context);


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
}
