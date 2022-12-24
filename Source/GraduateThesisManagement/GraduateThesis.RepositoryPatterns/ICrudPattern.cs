using GraduateThesis.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
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
        Task<Pagination<TOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword);
        Task<List<TOutput>> GetListAsync(int count = 200);
        Task<TOutput> GetAsync(T_ID id);
        Task<DataResponse<TOutput>> CreateAsync(TInput input);
        Task<DataResponse<TOutput>> UpdateAsync(T_ID id, TInput input);
        Task<DataResponse> BatchDeleteAsync(T_ID id);
        Task<DataResponse> ForceDeleteAsync(T_ID id);
        Task<int> CountAsync();
        Task<IWorkbook> ExportToSpreadsheetAsync(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties);

        Pagination<TOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword);
        List<TOutput> GetList(int count = 200);
        TOutput Get(T_ID id);
        DataResponse<TOutput> Create(TInput input);
        DataResponse<TOutput> Update(T_ID id, TInput input);
        DataResponse BatchDelete(T_ID id);
        DataResponse ForceDelete(T_ID id);
        int Count();
        IWorkbook ExportToSpreadsheet(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties);
    }
}
