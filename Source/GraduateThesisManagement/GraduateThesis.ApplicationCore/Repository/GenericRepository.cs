using ExcelDataReader;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.File;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Uuid;
using GraduateThesis.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable

namespace GraduateThesis.ApplicationCore.Repository;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>

public partial class GenericRepository<TEntity, TInput, TOutput>
    where TEntity : class
    where TInput : class
    where TOutput : class
{
    private DbContext _context;
    private DbSet<TEntity> _dbSet;
    private Converter _converter;
    private ExpressionBuilder _expressionBuilder;
    private PaginationHelper<TEntity, TOutput> _paginationHelper;
    private ImportHelper<TEntity> _importHelper;
    private ExportHelper<TOutput> _exportHelper;
    private Expression<Func<TEntity, object>>[] _navigationPropertyPaths;

    public GenericRepository(DbContext context, DbSet<TEntity> dbSet)
    {
        _context = context;
        _dbSet = dbSet;
        _converter = new Converter();
        _expressionBuilder = new ExpressionBuilder();
        _paginationHelper = new PaginationHelper<TEntity, TOutput>(dbSet, _expressionBuilder);
        _exportHelper = new ExportHelper<TOutput>();
        _importHelper = new ImportHelper<TEntity>(context, dbSet, _converter);
        _navigationPropertyPaths = null;
    }


    #region include (EF)

    public void IncludeMany(params Expression<Func<TEntity, object>>[] navigationPropertyPaths)
    {
        if (navigationPropertyPaths != null)
        {
            _navigationPropertyPaths = navigationPropertyPaths;
            _paginationHelper.IncludeMany(navigationPropertyPaths);
        }
    }

    #endregion

    #region get records method

    public Pagination<TOutput> GetPagination(Pagination<TOutput> pagination, Expression<Func<TEntity, TOutput>> selector)
    {
        IQueryable<TEntity> queryable = _paginationHelper.GetQueryableForPagination(pagination.OrderBy, pagination.OrderOptions, pagination.SearchKeyword);
        pagination.TotalItemCount = queryable.Count();

        return _paginationHelper.GetPagination(queryable, pagination, selector);
    }

    public List<TOutput> GetList(int count, Expression<Func<TEntity, TOutput>> selector)
    {
        return _dbSet.Take(count).IncludeMultiple(_navigationPropertyPaths)
            .Where("x => x.IsDeleted == false").Select(selector).ToList();
    }

    public TOutput GetByPk(params object[] keyValues)
    {
        TEntity entity = _dbSet.Find(keyValues);
        if (entity == null)
            return default(TOutput)!;

        return _converter.To<TEntity, TOutput>(entity);
    }

    public TOutput Get(string propertyName, object value, Expression<Func<TEntity, TOutput>> selector)
    {
        return _dbSet.IncludeMultiple(_navigationPropertyPaths)
            .Where(_expressionBuilder.GetWhereExpString("x", propertyName, value))
            .Select(selector).SingleOrDefault();
    }

    #endregion


    #region create new records method

    public DataResponse<TOutput> Create(TInput input, GenerateUidOptions generateUidOptions)
    {
        TEntity entity = _converter.To<TInput, TEntity>(input);
        Type entityType = entity.GetType();

        PropertyInfo idPropertyInfo = entityType.GetProperty("Id");
        if (idPropertyInfo == null)
            return new DataResponse<TOutput>
            {
                Status = DataResponseStatus.Failed,
                Message = "Property named 'ID' not found"
            };

        object id = idPropertyInfo.GetValue(entity, null);
        if (idPropertyInfo.PropertyType == typeof(string) && generateUidOptions == GenerateUidOptions.ShortUid)
            idPropertyInfo.SetValue(entity, UidHelper.GetShortUid());

        if (idPropertyInfo.PropertyType == typeof(string) && generateUidOptions == GenerateUidOptions.MicrosoftUid)
            idPropertyInfo.SetValue(entity, UidHelper.GetMicrosoftUid());

        PropertyInfo createdAtPropertyInfo = entityType.GetProperty("CreatedAt");
        if (createdAtPropertyInfo != null)
            createdAtPropertyInfo.SetValue(entity, DateTime.Now);

        _dbSet.Add(entity);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse<TOutput>
        {
            Status = DataResponseStatus.Success,
            Data = _converter.To<TEntity, TOutput>(entity)
        };
    }

    public DataResponse BulkInsert(IEnumerable<TInput> inputs, Func<TInput, TEntity> predicate)
    {
        _dbSet.BulkInsert(_converter.To<TInput, TEntity>(inputs, predicate));
        _context.BulkSaveChanges();

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    #endregion


    #region update records method

    public DataResponse<TOutput> Update(object id, TInput input)
    {
        TEntity entity_fromDb = _dbSet.Find(id);
        if (entity_fromDb == null)
            return new DataResponse<TOutput> { Status = DataResponseStatus.NotFound };

        Type entityType = entity_fromDb.GetType();
        foreach (PropertyInfo inputProperty in input.GetType().GetProperties())
        {
            PropertyInfo entityPropertyInfo = entityType.GetProperty(inputProperty.Name);
            if (entityPropertyInfo != null)
                entityPropertyInfo.SetValue(entity_fromDb, inputProperty.GetValue(input));
        }

        PropertyInfo updatedAtPropertyInfo = entityType.GetProperty("UpdatedAt");
        if (updatedAtPropertyInfo != null)
            updatedAtPropertyInfo.SetValue(entity_fromDb, DateTime.Now);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse<TOutput>
        {
            Status = DataResponseStatus.Success,
            Data = _converter.To<TEntity, TOutput>(entity_fromDb)
        };
    }

    #endregion


    #region delete records method

    public DataResponse BatchDelete(object id)
    {
        TEntity entity = _dbSet.Find(id);
        if (entity == null)
            return new DataResponse { Status = DataResponseStatus.NotFound };

        Type entityType = entity.GetType();
        PropertyInfo isDeletedPropertyInfo = entityType.GetProperty("IsDeleted");
        if (isDeletedPropertyInfo == null)
            return new DataResponse<TOutput>
            {
                Status = DataResponseStatus.Failed,
                Message = "Property named 'IsDeleted' not found"
            };

        isDeletedPropertyInfo.SetValue(entity, true);

        PropertyInfo deletedAtPropertyInfo = entityType.GetProperty("DeletedAt");
        if (deletedAtPropertyInfo != null)
            deletedAtPropertyInfo.SetValue(entity, DateTime.Now);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public DataResponse ForceDelete(object id)
    {
        TEntity entity = _dbSet.Find(id);
        if (entity == null)
            return new DataResponse { Status = DataResponseStatus.NotFound };

        Type entityType = entity.GetType();
        PropertyInfo isDeletedPropertyInfo = entityType.GetProperty("IsDeleted");
        if (isDeletedPropertyInfo != null)
        {
            bool isDeleted = (bool)isDeletedPropertyInfo.GetValue(entity)!;
            if (!isDeleted)
                return new DataResponse { Status = DataResponseStatus.Failed };
        }

        _dbSet.Remove(entity);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    public DataResponse ForceDelete(object id, Func<bool> predicate)
    {
        if (!predicate())
            return new DataResponse { Status = DataResponseStatus.HasConstraint };

        return ForceDelete(id);
    }

    public DataResponse ForceDeleteMany(object[] idList)
    {
        List<TEntity> entities = new List<TEntity>();
        foreach (object id in idList)
        {
            TEntity entity = _dbSet.Find(id);
            if (entity == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };

            entities.Add(entity);
        }

        _dbSet.RemoveRange(entities);

        int affected = _context.SaveChanges();
        if (affected == 0)
            return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

        return new DataResponse { Status = DataResponseStatus.Success };
    }

    #endregion


    #region count records method

    public int Count()
    {
        return _dbSet.Count();
    }

    public long LongCount()
    {
        return _dbSet.LongCount();
    }

    #endregion


    #region export to files

    public byte[] Export(RecordFilter recordFilter, ExportMetadata exportMetadata, Expression<Func<TEntity, TOutput>> selector)
    {
        List<TOutput> outputs = _dbSet.Where(recordFilter.BuildExpString("p", null))
            .Take(exportMetadata.MaxRecordNumber).Select(selector).ToList();

        if (exportMetadata.TypeOptions == ExportTypeOptions.XLS || exportMetadata.TypeOptions == ExportTypeOptions.XLSX)
            return _exportHelper.ExportToSpreadsheet(outputs, exportMetadata.TypeOptions, exportMetadata.SheetName, exportMetadata.IncludeProperties);
        else
            throw new Exception("This file type is not supported");
    }

    #endregion


    #region import from files

    public DataResponse Import(Stream stream, ImportMetadata importMetadata, ImportSelector<TEntity> importSelector)
    {
        if(importMetadata.TypeOptions == ImportTypeOptions.XLS || importMetadata.TypeOptions == ImportTypeOptions.XLS)
        {
            if (importSelector.SimpleImportSpreadsheet != null)
                return _importHelper.ImportFromSpreadsheet(stream, importMetadata.StartFromRow, importSelector.SimpleImportSpreadsheet);
            else if (importSelector.SimpleImportSpreadsheet != null)
                return _importHelper.ImportFromSpreadsheet(stream, importMetadata.StartFromRow, importMetadata.SheetName, importSelector.AdvancedImportSpreadsheet);
            else
                throw new Exception("No matching selector found!");
        }
        else
        {
            throw new Exception("This file type is not supported!");
        }
    }

    #endregion
}
