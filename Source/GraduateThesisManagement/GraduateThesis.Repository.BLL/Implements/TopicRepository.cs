using GraduateThesis.Models;
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
    public class TopicRepository : ITopicRepository
    {
        private HUFI_graduatethesisContext _context;

        internal TopicRepository(HUFI_graduatethesisContext context)
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

        public DataResponse<TopicOutput> Create(TopicInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<TopicOutput>> CreateAsync(TopicInput input)
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

        public TopicOutput Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TopicOutput> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public List<TopicOutput> GetList(int count = 200)
        {
            throw new NotImplementedException();
        }

        public Task<List<TopicOutput>> GetListAsync(int count = 200)
        {
            throw new NotImplementedException();
        }

        public DataResponse<TopicOutput> Update(TopicInput input)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<TopicOutput>> UpdateAsync(TopicInput input)
        {
            throw new NotImplementedException();
        }
    }
}
