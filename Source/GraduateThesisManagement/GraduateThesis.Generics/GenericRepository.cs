using GraduateThesis.Common;
using GraduateThesis.ExtensionMethods;
using GraduateThesis.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraduateThesis.Generics
{
    public class GenericRepository<TContext, TEntity, TInput, TOutput>
        where TContext : class
        where TEntity : class
        where TInput : class
        where TOutput : class
    {
        private TContext _context;
        private Type _contextType;
        private DbSet<TEntity> _dbSet;
        private Expression<Func<TEntity, object>>[] _navigationPropertyPaths;
        public Expression<Func<TEntity, TOutput>> Selector { get; set; }

        public GenericRepository(TContext context, DbSet<TEntity> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
            _contextType = _context.GetType();
        }

        private bool IsValidType(Type type)
        {
            if (type == typeof(int))
                return true;

            if (type == typeof(long))
                return true;

            if (type == typeof(double))
                return true;

            if (type == typeof(string))
                return true;

            return false;
        }

        private object To(object input, Type outputType)
        {
            object output = Activator.CreateInstance(outputType)!;
            Type inputType = input.GetType();

            PropertyInfo[] properties = output.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                PropertyInfo inputProperty = inputType!.GetProperty(property.Name);
                if (inputProperty != null)
                {
                    property.SetValue(output, inputProperty.GetValue(input));
                }
            }

            return output;
        }

        public TOutput ToOutput(TEntity entity)
        {
            return (TOutput)To(entity, typeof(TOutput));
        }

        public TEntity ToEntity(TInput input)
        {
            return (TEntity)To(input, typeof(TEntity));
        }

        public void IncludeMany(params Expression<Func<TEntity, object>>[] navigationPropertyPaths)
        {
            if (navigationPropertyPaths != null)
                _navigationPropertyPaths = navigationPropertyPaths;
        }

        public string GetWhereExpressionForPagination(string prefix, string keyword)
        {
            StringBuilder expStringBuilder = new StringBuilder($"{prefix} => ");
            foreach(PropertyInfo property in typeof(TEntity).GetProperties())
            {
                if (IsValidType(property.PropertyType))
                    expStringBuilder.Append($"{prefix}.{property.Name}.Contains(\"{keyword}\") || ");
            }

            return expStringBuilder.ToString().TrimEnd(' ').TrimEnd('|').TrimEnd('|');
        }

        public string GetOrderByForPagination(string prefix, string propertyName)
        {
            if (typeof(TEntity).GetProperty(propertyName) == null)
                throw new Exception($"Property named '{propertyName}' not found");

            return $"{prefix} => {prefix}.{propertyName}";
        }

        #region get records method

        public Pagination<TOutput> GetPagination(int page, int pageSize, string orderBy, string keyword)
        {
            int n = (page - 1) * pageSize;
            int totalItemCount = _dbSet.Count();
            IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);

            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(keyword))
            {
                queryable = queryable.OrderBy(GetOrderByForPagination("p", orderBy))
                    .Where(GetWhereExpressionForPagination("p", keyword));
            }
            
            if (!string.IsNullOrEmpty(orderBy))
            {
                queryable = queryable.OrderBy(GetOrderByForPagination("p", orderBy));
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                queryable = queryable.Where(GetWhereExpressionForPagination("p", keyword));
            }

            List<TOutput> onePageOfData = queryable.Skip(n).Take(pageSize).Select(Selector).ToList();

            return new Pagination<TOutput>
            {
                Page = page,
                PageSize = pageSize,
                TotalItemCount = totalItemCount,
                Items = onePageOfData
            };
        }

        public async Task<Pagination<TOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, string keyword) 
        {
            int n = (page - 1) * pageSize;
            int totalItemCount = await _dbSet.CountAsync();
            IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);

            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(keyword))
            {
                queryable = queryable.OrderBy(GetOrderByForPagination("p", orderBy))
                    .Where(GetWhereExpressionForPagination("p", keyword));
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                queryable = queryable.OrderBy(GetOrderByForPagination("p", orderBy));
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                queryable = queryable.Where(GetWhereExpressionForPagination("p", keyword));
            }

            List<TOutput> onePageOfData = await queryable.Skip(n).Take(pageSize).Select(Selector).ToListAsync();

            return new Pagination<TOutput>
            {
                Page = page,
                PageSize = pageSize,
                TotalItemCount = totalItemCount,
                Items = onePageOfData
            };
        }

        public List<TOutput> GetList(int count = 200)
        {
            return _dbSet.Take(count).IncludeMultiple(_navigationPropertyPaths).Select(Selector).ToList();
        }

        public async Task<List<TOutput>> GetListAsync(int count = 200)
        {
            return await _dbSet.Take(count).IncludeMultiple(_navigationPropertyPaths).Select(Selector).ToListAsync();
        }

        public TOutput GetById(object id)
        {
            TEntity entity = _dbSet.Find(id);
            if (entity == null)
                return default(TOutput)!;

            return ToOutput(entity);
        }

        public async Task<TOutput> GetByIdAsync(object id)
        {
            TEntity entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return default(TOutput)!;

            return ToOutput(entity);
        }

        #endregion

        #region create new records method

        public DataResponse<TOutput> Create(TInput input, GenerateUIDOptions generateUIDOptions)
        {
            TEntity entity = ToEntity(input);

            PropertyInfo idPropertyInfo = entity.GetType().GetProperty("Id");
            if (idPropertyInfo == null)
                return new DataResponse<TOutput>
                {
                    Status = DataResponseStatus.Failed,
                    Message = "Property named 'ID' not found"
                };

            object id = idPropertyInfo.GetValue(entity, null);
            if (id is string && generateUIDOptions == GenerateUIDOptions.ShortUID)
                idPropertyInfo.SetValue(entity, UID.GetShortUID());

            if (id is string && generateUIDOptions == GenerateUIDOptions.MicrosoftUID)
                idPropertyInfo.SetValue(entity, UID.GetUUID());

            _dbSet.Add(entity);

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("SaveChanges", new Type[] {})!;
            int affected = (int)saveChangesMethodInfo.Invoke(_context, null)!;

            if (affected == 0)
                return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

            return new DataResponse<TOutput>
            {
                Status = DataResponseStatus.Success,
                Data = ToOutput(entity)
            };
        }

        public async Task<DataResponse<TOutput>> CreateAsync(TInput input, GenerateUIDOptions generateUIDOptions)
        {
            TEntity entity = ToEntity(input);

            PropertyInfo idPropertyInfo = entity.GetType().GetProperty("Id");
            if (idPropertyInfo == null)
                return new DataResponse<TOutput>
                {
                    Status = DataResponseStatus.Failed,
                    Message = "Property named 'ID' not found"
                };

            object id = idPropertyInfo.GetValue(entity, null);
            if (id is string && generateUIDOptions == GenerateUIDOptions.ShortUID)
                idPropertyInfo.SetValue(entity, UID.GetShortUID());

            if (id is string && generateUIDOptions == GenerateUIDOptions.MicrosoftUID)
                idPropertyInfo.SetValue(entity, UID.GetUUID());

            _dbSet.Add(entity);

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("SaveChangesAsync", new Type[] { typeof(CancellationToken) });
            Task<int> resultAsync = (Task<int>)saveChangesMethodInfo.Invoke(_context, new object[] { default(CancellationToken) });

            int affected = await resultAsync;
            if (affected == 0)
                return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

            return new DataResponse<TOutput>
            {
                Status = DataResponseStatus.Success,
                Data = ToOutput(entity)
            };
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

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("SaveChanges", new Type[] { })!;
            int affected = (int)saveChangesMethodInfo.Invoke(_context, null)!;

            if (affected == 0)
                return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

            return new DataResponse<TOutput> {
                Status = DataResponseStatus.Success,
                Data = ToOutput(entity_fromDb)
            };
        }

        public async Task<DataResponse<TOutput>> UpdateAsync(object id, TInput input)
        {
            TEntity entity_fromDb = await _dbSet.FindAsync(id);
            if (entity_fromDb == null)
                return new DataResponse<TOutput> { Status = DataResponseStatus.NotFound };

            Type entityType = entity_fromDb.GetType();
            foreach(PropertyInfo inputProperty in input.GetType().GetProperties())
            {
                PropertyInfo entityPropertyInfo = entityType.GetProperty(inputProperty.Name);
                if (entityPropertyInfo != null)
                    entityPropertyInfo.SetValue(entity_fromDb, inputProperty.GetValue(input));
            }

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("SaveChangesAsync", new Type[] { typeof(CancellationToken) });
            Task<int> resultAsync = (Task<int>)saveChangesMethodInfo.Invoke(_context, new object[] { default(CancellationToken) });

            int affected = await resultAsync;
            if (affected == 0)
                return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

            return new DataResponse<TOutput>
            {
                Status = DataResponseStatus.Success,
                Data = ToOutput(entity_fromDb)
            };
        }


        #endregion

        #region delete records method

        public DataResponse BatchDelete(object id)
        {
            TEntity entity = _dbSet.Find(id);
            if (entity == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };

            PropertyInfo isDeletedPropertyInfo = entity.GetType().GetProperty("IsDeleted");
            if (isDeletedPropertyInfo == null)
                return new DataResponse<TOutput>
                {
                    Status = DataResponseStatus.Failed,
                    Message = "Property named 'IsDeleted' not found"
                };

            isDeletedPropertyInfo.SetValue(entity, true);

            return new DataResponse { Status = DataResponseStatus.Success };
        }

        public async Task<DataResponse> BatchDeleteAsync(object id)
        {
            TEntity entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };

            PropertyInfo isDeletedPropertyInfo = entity.GetType().GetProperty("IsDeleted");
            if (isDeletedPropertyInfo == null)
                return new DataResponse<TOutput>
                {
                    Status = DataResponseStatus.Failed,
                    Message = "Property named 'IsDeleted' not found"
                };

            isDeletedPropertyInfo.SetValue(entity, true);

            return new DataResponse { Status = DataResponseStatus.Success };
        }

        #endregion

        #region count records method

        public int Count()
        {
            return _dbSet.Count();
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public int Count(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.Count(where);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            return await _dbSet.CountAsync(where);
        }

        #endregion
    }
}
