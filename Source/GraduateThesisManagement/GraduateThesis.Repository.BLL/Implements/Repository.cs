﻿using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using Microsoft.EntityFrameworkCore;
using System;

namespace GraduateThesis.Repository.BLL.Implements
{
    public class Repository : IRepository
    {
        private HUFI_graduatethesisContext _context;

        public Repository(Action<DbContextOptionsBuilder> optionsAction)
        {
            DbContextOptionsBuilder<HUFI_graduatethesisContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<HUFI_graduatethesisContext>();
            optionsAction.Invoke(dbContextOptionsBuilder);
            _context = new HUFI_graduatethesisContext(dbContextOptionsBuilder.Options);
        }

        private bool disposedValue;

        public IStudentRepository StudentRepository => new StudentRepository(_context);

        public IStudentClassRepository StudentClassRepository => new StudentClassRepository(_context);

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