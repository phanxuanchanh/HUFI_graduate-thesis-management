using GraduateThesis.Common;
using GraduateThesis.ExtensionMethods;
using GraduateThesis.Models;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        #region get records method

        private string GetWhereExpString(string prefix, string[] conditions, string keyword)
        {
            StringBuilder expStringBuilder = new StringBuilder($"{prefix} => ");
            PropertyInfo[] properties = typeof(TEntity).GetProperties()
                .Where(p => p.PropertyType == typeof(string)).ToArray();

            if (properties.Length == 0)
            {
                expStringBuilder.Append($"{prefix}.IsDeleted == false");
                return expStringBuilder.ToString();
            }

            expStringBuilder.Append("(");
            int count = 1;
            foreach (PropertyInfo property in properties)
            {
                if (count == properties.Length)
                    expStringBuilder.Append($"{prefix}.{property.Name}.Contains(\"{keyword}\")");
                else
                    expStringBuilder.Append($"{prefix}.{property.Name}.Contains(\"{keyword}\") || ");

                count++;
            }

            expStringBuilder.Append(") && ");
            foreach(string condition in conditions)
            {
                expStringBuilder.Append($"{prefix}.{condition} && ");
            }

            expStringBuilder.Append($"{prefix}.IsDeleted == false");

            return expStringBuilder.ToString();
        }

        private string GetWhereExpString(string prefix, string keyword)
        {
            StringBuilder expStringBuilder = new StringBuilder($"{prefix} => ");
            PropertyInfo[] properties = typeof(TEntity).GetProperties()
                .Where(p => p.PropertyType == typeof(string)).ToArray();

            if(properties.Length == 0)
            {
                expStringBuilder.Append($"{prefix}.IsDeleted == false");
                return expStringBuilder.ToString();
            }

            expStringBuilder.Append("(");
            int count = 1;
            foreach (PropertyInfo property in properties)
            {
                if (count == properties.Length)
                    expStringBuilder.Append($"{prefix}.{property.Name}.Contains(\"{keyword}\")");
                else
                    expStringBuilder.Append($"{prefix}.{property.Name}.Contains(\"{keyword}\") || ");

                count++;
            }

            expStringBuilder.Append($") && {prefix}.IsDeleted == false");

            return expStringBuilder.ToString();
        }

        private string GetWhereExpString(string prefix, string searchBy, object value)
        {
            StringBuilder expStringBuilder = new StringBuilder($"{prefix} => ");
            PropertyInfo property = typeof(TEntity).GetProperty(searchBy);
            if(property == null)
                throw new Exception($"Property named '{searchBy}' not found");

            if (property.PropertyType == typeof(string))
            {
                expStringBuilder.Append($"{prefix}{searchBy}.Contains(\"{value}\")");
            }
            else if (property.PropertyType == typeof(bool))
            {
                string boolValueAsString = ((bool)value) ? "true" : "false";
                expStringBuilder.Append($"{prefix}{searchBy} == {boolValueAsString}");
            }
            else if(
                property.PropertyType == typeof(int) || property.PropertyType == typeof(long)
                || property.PropertyType == typeof(float) || property.PropertyType == typeof(double)
            )
            {
                expStringBuilder.Append($"{prefix}{searchBy} == {value}");
            }
            else
            {
                throw new Exception($"This data type is not supported!");
            }

            expStringBuilder.Append($"&& {prefix}.IsDeleted == false");

            return expStringBuilder.ToString();
        }

        private string GetOrderByExpString(string prefix, string propertyName)
        {
            if (typeof(TEntity).GetProperty(propertyName) == null)
                throw new Exception($"Property named '{propertyName}' not found");

            return $"{prefix} => {prefix}.{propertyName}";
        }

        private Expression<Func<TEntity, object>> GetOrderByExpression(string prefix, string propertyName)
        {
            string orderByExpString = GetOrderByExpString(prefix, propertyName);
            Expression<Func<TEntity, object>> orderExpression = DynamicExpressionParser
                .ParseLambda<TEntity, object>(new ParsingConfig(), true, orderByExpString);

            return orderExpression;
        }

        private IOrderedQueryable<TEntity> GetOrderedQueryable(IQueryable<TEntity> queryable, string orderBy, OrderOptions orderOptions)
        {
            if (string.IsNullOrEmpty(orderBy))
                return queryable.OrderBy(GetOrderByExpString("p", "Id"));

            if (orderOptions == OrderOptions.ASC)
                return queryable.OrderBy(GetOrderByExpString("p", orderBy));

            return queryable.OrderByDescending(GetOrderByExpression("p", orderBy));
        }

        private IQueryable<TEntity> GetQueryableForPagination(string orderBy, OrderOptions orderOptions, string keyword)
        {
            IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);
            if (!string.IsNullOrEmpty(keyword))
            {
                queryable = queryable.Where(GetWhereExpString("p", keyword));
            }

            queryable = GetOrderedQueryable(queryable, orderBy, orderOptions);  

            return queryable;
        }

        private IQueryable<TEntity> GetQueryableForPagination(string orderBy, OrderOptions orderOptions, string filterBy, object filterValue)
        {
            IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);
            if (!string.IsNullOrEmpty(filterBy))
            {
                queryable = queryable.Where(GetWhereExpString("p", filterBy, filterValue));
            }

            queryable = GetOrderedQueryable(queryable, orderBy, orderOptions);

            return queryable;
        }

        private IQueryable<TEntity> GetQueryableForPagination(string orderBy, OrderOptions orderOptions, string[] conditions, string keyword)
        {
            if (conditions == null)
                throw new Exception("'conditions' must not be null!");

            if (conditions.Length == 0)
                throw new Exception("'conditions' must not be empty!");

            IQueryable<TEntity> queryable = _dbSet.IncludeMultiple(_navigationPropertyPaths);
            if (!string.IsNullOrEmpty(keyword))
            {
                queryable = queryable.Where(GetWhereExpString("p", conditions, keyword));
            }

            queryable = GetOrderedQueryable(queryable, orderBy, orderOptions);

            return queryable;
        }

        private Pagination<TOutput> GetPagination(IQueryable<TEntity> queryable, int page, int pageSize, int totalItemCount)
        {
            int n = (page - 1) * pageSize;
            List<TOutput> onePageOfData = queryable.Skip(n).Take(pageSize).Select(PaginationSelector).ToList();

            return new Pagination<TOutput>
            {
                Page = page,
                PageSize = pageSize,
                TotalItemCount = totalItemCount,
                Items = onePageOfData
            };
        }

        private async Task<Pagination<TOutput>> GetPaginationAsync(IQueryable<TEntity> queryable, int page, int pageSize, int totalItemCount)
        {
            int n = (page - 1) * pageSize;
            List<TOutput> onePageOfData = await queryable.Skip(n).Take(pageSize).Select(PaginationSelector).ToListAsync();

            return new Pagination<TOutput>
            {
                Page = page,
                PageSize = pageSize,
                TotalItemCount = totalItemCount,
                Items = onePageOfData
            };
        }

        public Pagination<TOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            IQueryable<TEntity> queryable = GetQueryableForPagination(orderBy, orderOptions, keyword);
            int totalItemCount = queryable.Count();

            return GetPagination(queryable, page, pageSize, totalItemCount);
        }

        public async Task<Pagination<TOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword)
        {
            IQueryable<TEntity> queryable = GetQueryableForPagination(orderBy, orderOptions, keyword);
            int totalItemCount = await queryable.CountAsync();

            return await GetPaginationAsync(queryable, page, pageSize, totalItemCount);
        }

        public Pagination<TOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string filterBy, object filterValue)
        {
            IQueryable<TEntity> queryable = GetQueryableForPagination(orderBy, orderOptions, filterBy, filterValue);
            int totalItemCount = queryable.Count();

            return GetPagination(queryable, page, pageSize, totalItemCount);
        }

        public async Task<Pagination<TOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string filterBy, object filterValue)
        {
            IQueryable<TEntity> queryable = GetQueryableForPagination(orderBy, orderOptions, filterBy, filterValue);
            int totalItemCount = await queryable.CountAsync();

            return await GetPaginationAsync(queryable, page, pageSize, totalItemCount);
        }

        public Pagination<TOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string[] conditions, string keyword)
        {
            IQueryable<TEntity> queryable = GetQueryableForPagination(orderBy, orderOptions, conditions, keyword);
            int totalItemCount = queryable.Count();

            return GetPagination(queryable, page, pageSize, totalItemCount);
        }

        public async Task<Pagination<TOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string[] conditions, string keyword)
        {
            IQueryable<TEntity> queryable = GetQueryableForPagination(orderBy, orderOptions, conditions, keyword);
            int totalItemCount = await queryable.CountAsync();

            return await GetPaginationAsync(queryable, page, pageSize, totalItemCount);
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

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("SaveChanges", new Type[] { })!;
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
            if ( generateUIDOptions == GenerateUIDOptions.ShortUID)
                idPropertyInfo.SetValue(entity, UID.GetShortUID());

            if ( generateUIDOptions == GenerateUIDOptions.MicrosoftUID)
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

        public DataResponse BulkInsert(IEnumerable<TInput> inputs, GenerateUIDOptions generateUIDOptions)
        {
            DateTime currentDateTime = DateTime.Now;

            IEnumerable<TEntity> entities = inputs.Select(s =>
            {
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

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("BulkSaveChanges", new Type[] { });
            int affected = (int)saveChangesMethodInfo.Invoke(_context, null);

            if (affected == 0)
                return new DataResponse { Status = DataResponseStatus.Failed };

            return new DataResponse { Status = DataResponseStatus.Success };
        }

        public async Task<DataResponse> BulkInsertAsync(IEnumerable<TInput> inputs, GenerateUIDOptions generateUIDOptions)
        {
            DateTime currentDateTime = DateTime.Now;

            IEnumerable<TEntity> entities = inputs.Select(s =>
            {
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

        public DataResponse ImportFromSpreadsheet(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName)
        {
            IWorkbook workbook;
            if (spreadsheetTypeOptions == SpreadsheetTypeOptions.XLS)
                workbook = new HSSFWorkbook(stream);
            else
                workbook = new XSSFWorkbook(stream);

            ISheet sheet = workbook.GetSheet(sheetName);
            List<TEntity> entities = new List<TEntity>();
            List<TInput> inputs = new List<TInput>();
            Type inputType = typeof(TInput);
            PropertyInfo[] inputProperties = inputType.GetProperties();

            for(int rowIndex = 1; rowIndex < sheet.LastRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                TInput input = Activator.CreateInstance<TInput>();
                for (int colIndex = 0; colIndex < row.LastCellNum; colIndex++)
                {
                    //...
                    inputs.Add(input);
                }
            }

            ImportFromSpreadsheet(stream, spreadsheetTypeOptions, sheetName, x =>
            {

                return default(TEntity);
            });

            return BulkInsert(inputs, GenerateUIDOptions.ShortUID);
        }

        public DataResponse ImportFromSpreadsheet(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, Func<IRow, TEntity> predicate)
        {
            IWorkbook workbook;
            if (spreadsheetTypeOptions == SpreadsheetTypeOptions.XLS)
                workbook = new HSSFWorkbook(stream);
            else
                workbook = new XSSFWorkbook(stream);

            ISheet sheet = workbook.GetSheet(sheetName);
            List<TEntity> entities = new List<TEntity>();

            for (int rowIndex = 1; rowIndex < sheet.LastRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                TEntity entity = predicate(row);
                entities.Add(entity);
            }

            _dbSet.BulkInsert(entities);

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("BulkSaveChanges", new Type[] { });
            int affected = (int)saveChangesMethodInfo.Invoke(_context, null);

            if (affected == 0)
                return new DataResponse { Status = DataResponseStatus.Failed };

            return new DataResponse { Status = DataResponseStatus.Success };
        }

        public async Task<DataResponse> ImportFromSpreadsheetAsync(Stream stream, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, Func<IRow, TEntity> predicate)
        {
            IWorkbook workbook;
            if (spreadsheetTypeOptions == SpreadsheetTypeOptions.XLS)
                workbook = new HSSFWorkbook(stream);
            else
                workbook = new XSSFWorkbook(stream);

            ISheet sheet = workbook.GetSheet(sheetName);
            List<TEntity> entities = new List<TEntity>();

            for (int rowIndex = 1; rowIndex < sheet.LastRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                TEntity entity = predicate(row);
                entities.Add(entity);
            }

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

            return new DataResponse<TOutput>
            {
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
            foreach (PropertyInfo inputProperty in input.GetType().GetProperties())
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

        private void SetDataToSheet(ISheet sheet, ICellStyle cellStyle, IEnumerable<TOutput> outputs, string[] includeProperties)
        {
            int rowIndex = 1;
            int cellIndex = 0;
            Type outputType = typeof(TOutput);
            PropertyInfo[] outputProperties = outputType.GetProperties();

            foreach (TOutput output in outputs)
            {
                IRow row = sheet.CreateRow(rowIndex);
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
                rowIndex++;
            }

            for (int i = 0; i < outputProperties.Length; i++)
            {
                sheet.AutoSizeColumn(i);
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

        private IWorkbook ExportToSpreadsheet(IEnumerable<TOutput> outputs, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
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

        private async Task<IWorkbook> ExportToSpreadsheetAsync(IEnumerable<TOutput> outputs, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
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

        public IWorkbook ExportToSpreadsheet(int count, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            List<TOutput> outputs = GetList(count);
            return ExportToSpreadsheet(outputs, spreadsheetTypeOptions, sheetName, includeProperties);
        }

        public async Task<IWorkbook> ExportToSpreadsheetAsync(int count, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            List<TOutput> outputs = await GetListAsync(count);
            return await ExportToSpreadsheetAsync(outputs, spreadsheetTypeOptions, sheetName, includeProperties);
        }

        public IWorkbook ExportToSpreadsheet(string filterBy, object filterValue, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            List<TOutput> outputs = GetPagination(1, 10000, "Id", OrderOptions.ASC, null).Items;
            return ExportToSpreadsheet(outputs, spreadsheetTypeOptions, sheetName, includeProperties);
        }

        public async Task<IWorkbook> ExportToSpreadsheetAsync(string filterBy, object filterValue, int count, SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            List<TOutput> outputs = (await GetPaginationAsync(1, count, "Id", OrderOptions.ASC, null)).Items;
            return await ExportToSpreadsheetAsync(outputs, spreadsheetTypeOptions, sheetName, includeProperties);
        }

        public IWorkbook ExportToSpreadsheet(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            List<TOutput> outputs = _dbSet
                .IncludeMultiple(_navigationPropertyPaths).Select(ListSelector).ToList();

            return ExportToSpreadsheet(outputs, spreadsheetTypeOptions, sheetName, includeProperties);
        }

        public async Task<IWorkbook> ExportToSpreadsheetAsync(SpreadsheetTypeOptions spreadsheetTypeOptions, string sheetName, string[] includeProperties)
        {
            List<TOutput> outputs = await _dbSet
                .IncludeMultiple(_navigationPropertyPaths).Select(ListSelector).ToListAsync();

            return await ExportToSpreadsheetAsync(outputs, spreadsheetTypeOptions, sheetName, includeProperties);
        }

        #endregion
    }
}
