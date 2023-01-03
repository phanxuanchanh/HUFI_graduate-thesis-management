using ExcelDataReader;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using System.Linq.Expressions;

#nullable disable

namespace GraduateThesis.ApplicationCore.Repository;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
/// <typeparam name="T_ID"></typeparam>

public abstract class SubRepository<TEntity, TInput, TOutput, T_ID> : ISubRepository<TInput, TOutput, T_ID>
    where TEntity : class
    where TInput : class
    where TOutput : class
{
    private bool _isApplyDefaultSelector;
    private Expression<Func<TEntity, TOutput>> _customSelector;
    private Func<IExcelDataReader, TEntity> _customSimpleImportSeletor;
    private Func<DataRow, TEntity> _customAdvancedImportSeletor;

    protected GenericRepository<TEntity, TInput, TOutput> _genericRepository;
    
    public GenerateUidOptions GenerateUidOptions { get; set; }

    public Expression<Func<TEntity, TOutput>> PaginationSelector { get; set; }
    public Expression<Func<TEntity, TOutput>> ListSelector { get; set; }
    public Expression<Func<TEntity, TOutput>> SingleSelector { get; set; }
    public Func<IExcelDataReader, TEntity> SimpleImportSelector { get; set; }
    public Func<DataRow, TEntity> AdvancedImportSelector { get; set; }

    public SubRepository(DbContext dbContext, DbSet<TEntity> dbSet)
    {
        _isApplyDefaultSelector = true;
        _customSelector = null;
        _customSimpleImportSeletor = null;
        _customAdvancedImportSeletor = null;
        _genericRepository = new GenericRepository<TEntity, TInput, TOutput>(dbContext, dbSet);

        ConfigureIncludes();
        ConfigureSelectors();
    }

    protected abstract void ConfigureIncludes();
    protected abstract void ConfigureSelectors();

    public void UseSelector(Expression<Func<TEntity, TOutput>> selector)
    {
        _isApplyDefaultSelector = false;
        _customSelector = selector;
    }

    public void UseSimpleImportSelector(Func<IExcelDataReader, TEntity> selector)
    {
        _isApplyDefaultSelector = false;
        _customSimpleImportSeletor = selector;
    }

    public void UseAdvancedImportSelector(Func<DataRow, TEntity> selector)
    {
        _isApplyDefaultSelector = false;
        _customAdvancedImportSeletor = selector;
    }

    public void IncludeMany(params Expression<Func<TEntity, object>>[] navigationPropertyPaths)
    {
        _genericRepository.IncludeMany(navigationPropertyPaths);
    }

    public virtual DataResponse BatchDelete(T_ID id)
    {
        return _genericRepository.BatchDelete(id);
    }

    public virtual async Task<DataResponse> BatchDeleteAsync(T_ID id)
    {
        return await _genericRepository.BatchDeleteAsync(id);
    }

    public virtual int Count()
    {
        return _genericRepository.Count();
    }

    public virtual async Task<int> CountAsync()
    {
        return await _genericRepository.CountAsync();
    }

    public virtual DataResponse<TOutput> Create(TInput input)
    {
        return _genericRepository.Create(input, GenerateUidOptions);
    }

    public virtual async Task<DataResponse<TOutput>> CreateAsync(TInput input)
    {
        return await _genericRepository.CreateAsync(input, GenerateUidOptions);
    }

    public virtual byte[] Export(RecordFilter recordFilter, ExportMetadata exportMetadata)
    {
        if (_isApplyDefaultSelector)
            return _genericRepository.Export(recordFilter, exportMetadata, ListSelector);

        _isApplyDefaultSelector = true;
        return _genericRepository.Export(recordFilter, exportMetadata, _customSelector);
    }

    public virtual async Task<byte[]> ExportAsync(RecordFilter recordFilter, ExportMetadata exportMetadata)
    {
        if (_isApplyDefaultSelector)
            return await _genericRepository.ExportAsync(recordFilter, exportMetadata, ListSelector);

        _isApplyDefaultSelector = true;
        return await _genericRepository.ExportAsync(recordFilter, exportMetadata, _customSelector);
    }

    public virtual DataResponse ForceDelete(T_ID id)
    {
        return _genericRepository.ForceDelete(id);
    }

    public virtual async Task<DataResponse> ForceDeleteAsync(T_ID id)
    {
        return await _genericRepository.ForceDeleteAsync(id);
    }

    public virtual TOutput Get(T_ID id)
    {
        if(_isApplyDefaultSelector)
            return _genericRepository.Get("Id", id, SingleSelector);

        _isApplyDefaultSelector = true;
        return _genericRepository.Get("Id", id, _customSelector);
    }

    public virtual async Task<TOutput> GetAsync(T_ID id)
    {
        if (_isApplyDefaultSelector)
            return await _genericRepository.GetAsync("Id", id, SingleSelector);

        _isApplyDefaultSelector = true;
        return await _genericRepository.GetAsync("Id", id, _customSelector);
    }

    public virtual List<TOutput> GetList(int count = 200)
    {
        if (_isApplyDefaultSelector)
            return _genericRepository.GetList(count, ListSelector);

        _isApplyDefaultSelector = true;
        return _genericRepository.GetList(count, _customSelector);
    }

    public virtual async Task<List<TOutput>> GetListAsync(int count = 200)
    {
        if (_isApplyDefaultSelector)
            return await _genericRepository.GetListAsync(count, ListSelector);

        _isApplyDefaultSelector = true;
        return await _genericRepository.GetListAsync(count, _customSelector);
    }

    public virtual Pagination<TOutput> GetPagination(Pagination<TOutput> pagination)
    {
        if (_isApplyDefaultSelector)
            return _genericRepository.GetPagination(pagination, PaginationSelector);

        _isApplyDefaultSelector = true;
        return _genericRepository.GetPagination(pagination, _customSelector);
    }

    public virtual async Task<Pagination<TOutput>> GetPaginationAsync(Pagination<TOutput> pagination)
    {
        if(_isApplyDefaultSelector)
            return await _genericRepository.GetPaginationAsync(pagination, PaginationSelector);

        _isApplyDefaultSelector = true;
        return await _genericRepository.GetPaginationAsync(pagination, _customSelector);
    }

    public virtual DataResponse Import(Stream stream, ImportMetadata importMetadata)
    {
        if (_isApplyDefaultSelector)
            return _genericRepository.Import(stream, importMetadata, new ImportSelector<TEntity>
            {
                SimpleImportSpreadsheet = SimpleImportSelector
            });

        _isApplyDefaultSelector = true;
        return _genericRepository.Import(stream, importMetadata, new ImportSelector<TEntity>
        {
            SimpleImportSpreadsheet = _customSimpleImportSeletor
        });
    }

    public virtual async Task<DataResponse> ImportAsync(Stream stream, ImportMetadata importMetadata)
    {
        if (_isApplyDefaultSelector)
            return await _genericRepository.ImportAsync(stream, importMetadata, new ImportSelector<TEntity>
            {
                SimpleImportSpreadsheet = SimpleImportSelector
            });

        _isApplyDefaultSelector = true;
        return await _genericRepository.ImportAsync(stream, importMetadata, new ImportSelector<TEntity>
        {
            SimpleImportSpreadsheet = _customSimpleImportSeletor
        });
    }

    public virtual DataResponse<TOutput> Update(T_ID id, TInput input)
    {
        return _genericRepository.Update(id, input);
    }

    public virtual async Task<DataResponse<TOutput>> UpdateAsync(T_ID id, TInput input)
    {
        return await _genericRepository.UpdateAsync(id, input);
    }

    public async Task<Pagination<TOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
    {
        Pagination<TOutput> pagination = new Pagination<TOutput>
        {
            Page = page,
            PageSize = pageSize,
            OrderBy = orderBy,
            OrderOptions = orderOptions,
            SearchKeyword = keyword
        };

        if(_isApplyDefaultSelector)
            return await _genericRepository.GetPaginationAsync(pagination, PaginationSelector);

        _isApplyDefaultSelector = true;
        return await _genericRepository.GetPaginationAsync(pagination, _customSelector);
    }

    public Pagination<TOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
    {
        Pagination<TOutput> pagination = new Pagination<TOutput>
        {
            Page = page,
            PageSize = pageSize,
            OrderBy = orderBy,
            OrderOptions = orderOptions,
            SearchKeyword = keyword
        };

        if (_isApplyDefaultSelector)
            return _genericRepository.GetPagination(pagination, PaginationSelector);

        _isApplyDefaultSelector = true;
        return _genericRepository.GetPagination(pagination, _customSelector);
    }
}
