﻿using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;

namespace GraduateThesis.ApplicationCore.Repository;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
/// <typeparam name="T_ID"></typeparam>

public interface ISubRepository<TInput, TOutput, T_ID>
    where TInput : class 
    where TOutput : class 
{
    Task<Pagination<TOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword);
    Task<Pagination<TOutput>> GetPaginationAsync(Pagination<TOutput> pagination);
    Task<List<TOutput>> GetListAsync(int count = 200);
    Task<TOutput> GetAsync(T_ID id);
    Task<DataResponse<TOutput>> CreateAsync(TInput input);
    Task<DataResponse<TOutput>> UpdateAsync(T_ID id, TInput input);
    Task<DataResponse> BatchDeleteAsync(T_ID id);
    Task<DataResponse> ForceDeleteAsync(T_ID id);
    Task<int> CountAsync();
    Task<byte[]> ExportAsync(RecordFilter recordFilter, ExportMetadata exportMetadata);
    Task<DataResponse> ImportAsync(Stream stream, ImportMetadata importMetadata);

    Pagination<TOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword);
    Pagination<TOutput> GetPagination(Pagination<TOutput> pagination);
    List<TOutput> GetList(int count = 200);
    TOutput Get(T_ID id);
    DataResponse<TOutput> Create(TInput input);
    DataResponse<TOutput> Update(T_ID id, TInput input);
    DataResponse BatchDelete(T_ID id);
    DataResponse ForceDelete(T_ID id);
    int Count();
    byte[] Export(RecordFilter recordFilter, ExportMetadata exportMetadata);
    DataResponse Import(Stream stream, ImportMetadata importMetadata);
}