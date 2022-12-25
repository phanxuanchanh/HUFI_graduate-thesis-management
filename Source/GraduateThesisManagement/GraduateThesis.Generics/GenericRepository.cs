using GraduateThesis.Common;
using GraduateThesis.ExtensionMethods;
using GraduateThesis.Models;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Z.EntityFramework.Extensions;

namespace GraduateThesis.Generics
{
    /// <summary>
    /// Author: phanxuanchanh.com (phanchanhvn)
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    
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

        public Expression<Func<TEntity, TOutput>> PaginationSelector { get; set; }
        public Expression<Func<TEntity, TOutput>> ListSelector { get; set; }
        public Expression<Func<TEntity, TOutput>> SingleSelector { get; set; }

        public GenericRepository(TContext context, DbSet<TEntity> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
            _contextType = _context.GetType();
        }

        private object To(object input, Type outputType)
        {
            object output = Activator.CreateInstance(outputType)!;
            Type inputType = input.GetType();

            PropertyInfo[] properties = output.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                PropertyInfo inputProperty = inputType.GetProperty(property.Name);
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

        private string GetWhereExpForPagination(string prefix, string keyword)
        {
            StringBuilder expStringBuilder = new StringBuilder($"{prefix} => (");
            foreach(PropertyInfo property in typeof(TEntity).GetProperties())
            {
                if(property.PropertyType == typeof(string))
                    expStringBuilder.Append($"{prefix}.{property.Name}.Contains(\"{keyword}\") || ");
            }

            string expression = expStringBuilder.ToString().TrimEnd(' ').TrimEnd('|').TrimEnd('|');
            return $"{expression}) && {prefix}.IsDeleted == false";
        }

        private string GetWhereExpForConditionalPagination(string prefix, string condition, string keyword)
        {
            StringBuilder expStringBuilder = new StringBuilder($"{prefix} => (");
            foreach (PropertyInfo property in typeof(TEntity).GetProperties())
            {
                if (property.PropertyType == typeof(string))
                    expStringBuilder.Append($"{prefix}.{property.Name}.Contains(\"{keyword}\") || ");
            }

            string expression = expStringBuilder.ToString().TrimEnd(' ').TrimEnd('|').TrimEnd('|');
            return $"{expression}) && {prefix}.{condition} && {prefix}.IsDeleted == false";
        }

        private string GetOrderByForPagination(string prefix, string propertyName)
        {
            if (typeof(TEntity).GetProperty(propertyName) == null)
                throw new Exception($"Property named '{propertyName}' not found");

            return $"{prefix} => {prefix}.{propertyName}";
        }

        #region get records method

        public Pagination<TOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            int n = (page - 1) * pageSize;
            int totalItemCount = 0;
            IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);

            if (string.IsNullOrEmpty(orderBy) && string.IsNullOrEmpty(keyword))
            {
                totalItemCount = _dbSet.Count();
                queryable = queryable.Where(GetWhereExpForPagination("p", keyword))
                        .OrderBy(GetOrderByForPagination("p", "Id"));
            }

            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(keyword))
            {
                totalItemCount = _dbSet.Where(GetWhereExpForPagination("p", keyword)).Count();
                if (orderOptions == OrderOptions.ASC)
                {
                    queryable = queryable.Where(GetWhereExpForPagination("p", keyword))
                        .OrderBy(GetOrderByForPagination("p", orderBy));
                }
                else
                {
                    Expression<Func<TEntity, object>> orderExpression = DynamicExpressionParser.ParseLambda<TEntity, object>(
                        new ParsingConfig(),
                        true,
                        GetOrderByForPagination("p", orderBy)
                    );
                    queryable = queryable.Where(GetWhereExpForPagination("p", keyword))
                        .OrderByDescending(orderExpression);

                }     
            }
            
            if (!string.IsNullOrEmpty(orderBy))
            {
                if (orderOptions == OrderOptions.ASC)
                {
                    queryable = queryable.OrderBy(GetOrderByForPagination("p", orderBy));
                }
                else
                {
                    Expression<Func<TEntity, object>> orderExpression = DynamicExpressionParser.ParseLambda<TEntity, object>(
                        new ParsingConfig(),
                        true,
                        GetOrderByForPagination("p", orderBy)
                    );
                    queryable = queryable.OrderByDescending(orderExpression);

                }
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                totalItemCount = _dbSet.Where(GetWhereExpForPagination("p", keyword)).Count();
                queryable = queryable.Where(GetWhereExpForPagination("p", keyword));
            }

            List<TOutput> onePageOfData = queryable.Skip(n).Take(pageSize).Select(PaginationSelector).ToList();

            return new Pagination<TOutput>
            {
                Page = page,
                PageSize = pageSize,
                TotalItemCount = totalItemCount,
                Items = onePageOfData
            };
        }

        public async Task<Pagination<TOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword) 
        {
            int n = (page - 1) * pageSize;
            int totalItemCount = 0;
            IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);

            if(string.IsNullOrEmpty(orderBy) && string.IsNullOrEmpty(keyword))
            {
                totalItemCount = await _dbSet.CountAsync();
                queryable = queryable.Where(GetWhereExpForPagination("p", keyword))
                        .OrderBy(GetOrderByForPagination("p", "Id"));
            }

            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(keyword))
            {
                totalItemCount = await _dbSet.Where(GetWhereExpForPagination("p", keyword)).CountAsync();
                if (orderOptions == OrderOptions.ASC)
                {
                    queryable = queryable.Where(GetWhereExpForPagination("p", keyword))
                        .OrderBy(GetOrderByForPagination("p", orderBy));
                }
                else
                {
                    Expression<Func<TEntity, object>> orderExpression = DynamicExpressionParser.ParseLambda<TEntity, object>(
                        new ParsingConfig(),
                        true,
                        GetOrderByForPagination("p", orderBy)
                    );
                    queryable = queryable.Where(GetWhereExpForPagination("p", keyword))
                        .OrderByDescending(orderExpression);

                }
            }
            
            if(!string.IsNullOrEmpty(orderBy))
            {
                if (orderOptions == OrderOptions.ASC)
                {
                    queryable = queryable.OrderBy(GetOrderByForPagination("p", orderBy));
                }
                else
                {
                    Expression<Func<TEntity, object>> orderExpression = DynamicExpressionParser.ParseLambda<TEntity, object>(
                        new ParsingConfig(),
                        true,
                        GetOrderByForPagination("p", orderBy)
                    );
                    queryable = queryable.OrderByDescending(orderExpression);
                }
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                totalItemCount = await _dbSet.Where(GetWhereExpForPagination("p", keyword)).CountAsync();
                queryable = queryable.Where(GetWhereExpForPagination("p", keyword))
                    .OrderBy(GetOrderByForPagination("p", "Id"));
            }

            List<TOutput> onePageOfData = await queryable.Skip(n).Take(pageSize).Select(PaginationSelector).ToListAsync();

            return new Pagination<TOutput>
            {
                Page = page,
                PageSize = pageSize,
                TotalItemCount = totalItemCount,
                Items = onePageOfData
            };
        }

        public Pagination<TOutput> GetConditionalPagination(string condition, int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            int n = (page - 1) * pageSize;
            int totalItemCount = 0;
            IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);

            if (string.IsNullOrEmpty(orderBy) && string.IsNullOrEmpty(keyword))
            {
                totalItemCount = _dbSet.Count();
                queryable = queryable.Where(GetWhereExpForConditionalPagination("p", condition, keyword))
                        .OrderBy(GetOrderByForPagination("p", "Id"));
            }

            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(keyword))
            {
                totalItemCount = _dbSet.Where(GetWhereExpForConditionalPagination("p", condition, keyword)).Count();
                if (orderOptions == OrderOptions.ASC)
                {
                    queryable = queryable.Where(GetWhereExpForConditionalPagination("p", condition, keyword))
                        .OrderBy(GetOrderByForPagination("p", orderBy));
                }
                else
                {
                    Expression<Func<TEntity, object>> orderExpression = DynamicExpressionParser.ParseLambda<TEntity, object>(
                        new ParsingConfig(),
                        true,
                        GetOrderByForPagination("p", orderBy)
                    );
                    queryable = queryable.Where(GetWhereExpForConditionalPagination("p", condition, keyword))
                        .OrderByDescending(orderExpression);

                }
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                if (orderOptions == OrderOptions.ASC)
                {
                    queryable = queryable.OrderBy(GetOrderByForPagination("p", orderBy));
                }
                else
                {
                    Expression<Func<TEntity, object>> orderExpression = DynamicExpressionParser.ParseLambda<TEntity, object>(
                        new ParsingConfig(),
                        true,
                        GetOrderByForPagination("p", orderBy)
                    );
                    queryable = queryable.OrderByDescending(orderExpression);

                }
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                totalItemCount = _dbSet.Where(GetWhereExpForConditionalPagination("p", condition, keyword)).Count();
                queryable = queryable.Where(GetWhereExpForConditionalPagination("p", condition, keyword));
            }

            List<TOutput> onePageOfData = queryable.Skip(n).Take(pageSize).Select(PaginationSelector).ToList();

            return new Pagination<TOutput>
            {
                Page = page,
                PageSize = pageSize,
                TotalItemCount = totalItemCount,
                Items = onePageOfData
            };
        }

        public async Task<Pagination<TOutput>> GetConditionalPaginationAsync(string condition, int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            int n = (page - 1) * pageSize;
            int totalItemCount = 0;
            IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);

            if (string.IsNullOrEmpty(orderBy) && string.IsNullOrEmpty(keyword))
            {
                totalItemCount = await _dbSet.CountAsync();
                queryable = queryable.Where(GetWhereExpForConditionalPagination("p", condition, keyword))
                        .OrderBy(GetOrderByForPagination("p", "Id"));
            }

            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(keyword))
            {
                totalItemCount = await _dbSet.Where(GetWhereExpForConditionalPagination("p", condition, keyword)).CountAsync();
                if (orderOptions == OrderOptions.ASC)
                {
                    queryable = queryable.Where(GetWhereExpForConditionalPagination("p", condition, keyword))
                        .OrderBy(GetOrderByForPagination("p", orderBy));
                }
                else
                {
                    Expression<Func<TEntity, object>> orderExpression = DynamicExpressionParser.ParseLambda<TEntity, object>(
                        new ParsingConfig(),
                        true,
                        GetOrderByForPagination("p", orderBy)
                    );
                    queryable = queryable.Where(GetWhereExpForConditionalPagination("p", condition, keyword))
                        .OrderByDescending(orderExpression);

                }
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                if (orderOptions == OrderOptions.ASC)
                {
                    queryable = queryable.OrderBy(GetOrderByForPagination("p", orderBy));
                }
                else
                {
                    Expression<Func<TEntity, object>> orderExpression = DynamicExpressionParser.ParseLambda<TEntity, object>(
                        new ParsingConfig(),
                        true,
                        GetOrderByForPagination("p", orderBy)
                    );
                    queryable = queryable.OrderByDescending(orderExpression);

                }
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                totalItemCount = await _dbSet.Where(GetWhereExpForConditionalPagination("p", condition, keyword)).CountAsync();
                queryable = queryable.Where(GetWhereExpForConditionalPagination("p", condition, keyword));
            }

            List<TOutput> onePageOfData = await queryable.Skip(n).Take(pageSize).Select(PaginationSelector).ToListAsync();

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
            return _dbSet.Take(count).IncludeMultiple(_navigationPropertyPaths)
                .Where("x => x.IsDeleted == false").Select(ListSelector).ToList();
        }

        public async Task<List<TOutput>> GetListAsync(int count = 200)
        {
            return await _dbSet.Take(count).IncludeMultiple(_navigationPropertyPaths)
                .Where("x => x.IsDeleted == false").Select(ListSelector).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition">Example: name == 'abc' => x.name == 'abc' </param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<TOutput> GetListByCondition(string condition, int count = 200)
        {
            return _dbSet.Take(count).IncludeMultiple(_navigationPropertyPaths)
                .Where($"x => x.{condition} && x.IsDeleted == false").Select(ListSelector).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition">Example: name == 'abc' => x.name == 'abc' </param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<TOutput>> GetListByConditionAsync(string condition, int count = 200)
        {
            return await _dbSet.Take(count).IncludeMultiple(_navigationPropertyPaths)
                .Where($"x => x.{condition} && x.IsDeleted == false").Select(ListSelector).ToListAsync();
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

        public TOutput Get(string propertyName, string value)
        {
            string valueInExpression = null;
            if (value.IsString())
                valueInExpression = $"\"{value}\"";

            if (value.IsNumber() || value.IsBool())
                valueInExpression = value.ToString();

            if (string.IsNullOrEmpty(valueInExpression))
                return null;

            string expression = $"x => (x.{propertyName} == {valueInExpression}) && x.IsDeleted == false";

            return _dbSet.IncludeMultiple(_navigationPropertyPaths)
                .Where(expression).Select(SingleSelector).SingleOrDefault();
        }

        public async Task<TOutput> GetAsync(string propertyName, object value)
        {
            string valueInExpression = null;
            if (value.IsString())
                valueInExpression = $"\"{value}\"";
            
            if (value.IsNumber() || value.IsBool())
                valueInExpression = value.ToString();

            if (string.IsNullOrEmpty(valueInExpression))
                return null;

            string expression = $"x => (x.{propertyName} == {valueInExpression}) && x.IsDeleted == false";

            return await _dbSet.IncludeMultiple(_navigationPropertyPaths)
                .Where(expression).Select(SingleSelector).SingleOrDefaultAsync();
        }

        #endregion

        #region create new records method

        public DataResponse<TOutput> Create(TInput input, GenerateUIDOptions generateUIDOptions)
        {
            TEntity entity = ToEntity(input);
            Type entityType = entity.GetType();

            PropertyInfo idPropertyInfo = entityType.GetProperty("Id");
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

            PropertyInfo createdAtPropertyInfo = entityType.GetProperty("CreatedAt");
            if (createdAtPropertyInfo != null)
                createdAtPropertyInfo.SetValue(entity, DateTime.Now);

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
            Type entityType = entity.GetType();

            PropertyInfo idPropertyInfo = entityType.GetProperty("Id");
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

            PropertyInfo createdAtPropertyInfo = entityType.GetProperty("CreatedAt");
            if (createdAtPropertyInfo != null)
                createdAtPropertyInfo.SetValue(entity, DateTime.Now);

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

        public DataResponse BulkInsert(IEnumerable<TInput> input, GenerateUIDOptions generateUIDOptions)
        {
            DateTime currentDateTime = DateTime.Now;
            
            IEnumerable<TEntity> entities = input.Select(s => {
                TEntity entity = ToEntity(s);
                Type entityType = entity.GetType();

                PropertyInfo idPropertyInfo = entityType.GetProperty("Id");
                if (idPropertyInfo != null)
                {
                    object id = idPropertyInfo.GetValue(s, null);
                    if (id is string && generateUIDOptions == GenerateUIDOptions.ShortUID)
                        idPropertyInfo.SetValue(entity, UID.GetShortUID());

                    if (id is string && generateUIDOptions == GenerateUIDOptions.MicrosoftUID)
                        idPropertyInfo.SetValue(entity, UID.GetUUID());
                }

                PropertyInfo createdAtPropertyInfo = entityType.GetProperty("CreatedAt");
                if (createdAtPropertyInfo != null)
                    createdAtPropertyInfo.SetValue(entity, currentDateTime);

                return entity;
            });

            _dbSet.BulkInsert(entities);

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("BulkSaveChanges", new Type[] {  });
            int affected = (int)saveChangesMethodInfo.Invoke(_context, null);

            if (affected == 0)
                return new DataResponse { Status = DataResponseStatus.Failed };

            return new DataResponse { Status = DataResponseStatus.Success };
        }

        public async Task<DataResponse> BulkInsertAsync(IEnumerable<TInput> input, GenerateUIDOptions generateUIDOptions)
        {
            DateTime currentDateTime = DateTime.Now;

            IEnumerable<TEntity> entities = input.Select(s => {
                TEntity entity = ToEntity(s);
                Type entityType = entity.GetType();

                PropertyInfo idPropertyInfo = entityType.GetProperty("Id");
                if (idPropertyInfo != null)
                {
                    object id = idPropertyInfo.GetValue(s, null);
                    if (id is string && generateUIDOptions == GenerateUIDOptions.ShortUID)
                        idPropertyInfo.SetValue(entity, UID.GetShortUID());

                    if (id is string && generateUIDOptions == GenerateUIDOptions.MicrosoftUID)
                        idPropertyInfo.SetValue(entity, UID.GetUUID());
                }

                PropertyInfo createdAtPropertyInfo = entityType.GetProperty("CreatedAt");
                if (createdAtPropertyInfo != null)
                    createdAtPropertyInfo.SetValue(entity, currentDateTime);

                return entity;
            });

            await _dbSet.BulkInsertAsync(entities);

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("BulkSaveChangesAsync", new Type[] { typeof(CancellationToken) });
            Task<int> resultAsync = (Task<int>)saveChangesMethodInfo.Invoke(_context, new object[] { default(CancellationToken) });

            int affected = await resultAsync;
            if (affected == 0)
                return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

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

            PropertyInfo updatedAtPropertyInfo = entityType.GetProperty("UpdatedAt");
            if (updatedAtPropertyInfo != null)
                updatedAtPropertyInfo.SetValue(entity_fromDb, DateTime.Now);

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

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("SaveChanges", new Type[] { })!;
            int affected = (int)saveChangesMethodInfo.Invoke(_context, null)!;

            if (affected == 0)
                return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

            return new DataResponse { Status = DataResponseStatus.Success };
        }

        public async Task<DataResponse> BatchDeleteAsync(object id)
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

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("SaveChangesAsync", new Type[] { typeof(CancellationToken) });
            Task<int> resultAsync = (Task<int>)saveChangesMethodInfo.Invoke(_context, new object[] { default(CancellationToken) });

            int affected = await resultAsync;
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

        #region export to files

        private ICellStyle SetDefaultCellStyle(IWorkbook workbook, bool isHeaderStyle)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();

            font.FontName = "Time New Roman";
            font.FontHeight = 30 * 20;
            font.FontHeightInPoints = 14;

            if (isHeaderStyle)
                font.IsBold = true;

            cellStyle.SetFont(font);

            return cellStyle;
        }

        private void SetSheetHeader(ISheet sheet, ICellStyle cellStyle, string[] includeProperties)
        {
            IRow headerRow = sheet.CreateRow(0);

            PropertyInfo[] outputProperties = typeof(TOutput).GetProperties();
            int cellIndex = 0;
            foreach (PropertyInfo propertyInfo in outputProperties)
            {
                if (includeProperties.Any(i => i == propertyInfo.Name))
                {
                    ICell cell = headerRow.CreateCell(cellIndex);
                    DisplayAttribute displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                    if (displayAttribute == null)
                        cell.SetCellValue(propertyInfo.Name);
                    else
                        cell.SetCellValue(displayAttribute.Name);

                    cell.CellStyle = cellStyle;
                    cellIndex++;
                }
            }

            for (int i = 0; i < outputProperties.Length; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }

        private void SetDataToSheet(ISheet sheet, ICellStyle cellStyle, List<TOutput> outputs, string[] includeProperties)
        {
            int rowIndex = 1;
            int cellIndex = 0;
            foreach (TOutput output in outputs)
            {
                IRow row = sheet.CreateRow(rowIndex);
                PropertyInfo[] outputProperties = typeof(TOutput).GetProperties();
                cellIndex = 0;
                foreach (PropertyInfo propertyInfo in outputProperties)
                {
                    if (includeProperties.Any(i => i == propertyInfo.Name))
                    {
                        ICell cell = row.CreateCell(cellIndex);
                        cell.CellStyle = cellStyle;
                        SetCellValue(cell, propertyInfo.GetValue(output));

                        cellIndex++;
                    }
                }

                for (int i = 0; i < outputProperties.Length; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                rowIndex++;
            }
        }

        private void SetCellValue(ICell cell, object value)
        {
            if (value.IsString())
                cell.SetCellValue(value.ToString());
            else if (value.IsBool())
                cell.SetCellValue((bool)value);
            else if (value.IsNumber())
                cell.SetCellValue(value.ToString());
            else
                cell.SetCellValue(value.ToString());
        }
        
        public IWorkbook ExportToSpreadsheet(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            List<TOutput> outputs = _dbSet
                .IncludeMultiple(_navigationPropertyPaths).Select(ListSelector).ToList();

            IWorkbook workbook;
            if (spreadsheetTypeOptions == SpreadsheetTypeOptions.XLS)
                workbook = new HSSFWorkbook();
            else
                workbook = new XSSFWorkbook();

            ICellStyle headerCellStyle = SetDefaultCellStyle(workbook, true);
            ICellStyle cellStyle = SetDefaultCellStyle(workbook, false);
            ISheet sheet = workbook.CreateSheet(sheetName);

            SetSheetHeader(sheet, headerCellStyle, includeProperties);
            SetDataToSheet(sheet, cellStyle, outputs, includeProperties);
            
            return workbook;
        }

        public async Task<IWorkbook> ExportToSpreadsheetAsync(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            List<TOutput> outputs = await _dbSet
                .IncludeMultiple(_navigationPropertyPaths).Select(ListSelector).ToListAsync();

            IWorkbook workbook;
            if (spreadsheetTypeOptions == SpreadsheetTypeOptions.XLS)
                workbook = new HSSFWorkbook();
            else
                workbook = new XSSFWorkbook();

            ICellStyle headerCellStyle = SetDefaultCellStyle(workbook, true);
            ICellStyle cellStyle = SetDefaultCellStyle(workbook, false);
            ISheet sheet = workbook.CreateSheet(sheetName);

            await Task.Run(() =>
            {
                SetSheetHeader(sheet, headerCellStyle, includeProperties);
                SetDataToSheet(sheet, cellStyle, outputs, includeProperties);
            });

            return workbook;
        }

        #endregion
    }
}
