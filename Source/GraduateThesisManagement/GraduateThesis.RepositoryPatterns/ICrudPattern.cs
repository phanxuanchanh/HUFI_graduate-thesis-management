using GraduateThesis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GraduateThesis.RepositoryPatterns
{
    /// <summary>
    /// Author: phanxuanchanh.com (phanchanhvn)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <typeparam name="T_ID"></typeparam>
 
    public interface ICrudPattern<TEntity, TInput, TOutput, T_ID> 
        where TEntity: class 
        where TInput : class 
        where TOutput : class 
        where T_ID : IConvertible
    {
        Task<List<TOutput>> GetListAsync(int count = 200);
        Task<TOutput> GetAsync(T_ID id);
        Task<DataResponse<TOutput>> CreateAsync(TInput input);
        Task<DataResponse<TOutput>> UpdateAsync(TInput input);
        Task<DataResponse> BatchDeleteAsync(T_ID id);
        Task<DataResponse> ForceDeleteAsync(T_ID id);
        Task<int> CountAsync();

        List<TOutput> GetList(int count = 200);
        TOutput Get(T_ID id);
        DataResponse<TOutput> Create(TInput input);
        DataResponse<TOutput> Update(TInput input);
        DataResponse BatchDelete(T_ID id);
        DataResponse ForceDelete(T_ID id);
        int Count();
    }
}
