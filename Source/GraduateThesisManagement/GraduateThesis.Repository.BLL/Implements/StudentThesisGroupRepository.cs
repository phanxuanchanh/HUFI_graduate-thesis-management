﻿using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements
{
    public class StudentThesisGroupRepository : IStudentThesisGroupRepository
    {
        private HUFI_graduatethesisContext _context;

        public StudentThesisGroupRepository(HUFI_graduatethesisContext context)
        {
            _context = context;
        }

        public DataResponse BatchDelete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse> BatchDeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public DataResponse<StudentThesisGroupOutput> Create(StudentThesisGroupInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<StudentThesisGroupOutput>> CreateAsync(StudentThesisGroupInput input)
        {
            throw new NotImplementedException();
        }

        public DataResponse ForceDelete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse> ForceDeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public StudentThesisGroupOutput Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<StudentThesisGroupOutput> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public List<StudentThesisGroupOutput> GetList(int count = 200)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentThesisGroupOutput>> GetListAsync(int count = 200)
        {
            throw new NotImplementedException();
        }

        public DataResponse<StudentThesisGroupOutput> Update(StudentThesisGroupInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<StudentThesisGroupOutput>> UpdateAsync(StudentThesisGroupInput input)
        {
            throw new NotImplementedException();
        }
    }
}
