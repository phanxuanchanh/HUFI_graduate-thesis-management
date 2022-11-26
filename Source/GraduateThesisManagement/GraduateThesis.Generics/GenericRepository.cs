using GraduateThesis.ExtensionMethods;
using GraduateThesis.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        private Expression<Func<TEntity, object>>[] _navigationPropertyPaths = default!;

        public Expression<Func<TEntity, TOutput>> Selector { get; set; } = default!;

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

        #region get records method

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

        public DataResponse<TOutput> Create(TInput input)
        {
            TEntity entity = ToEntity(input);

            PropertyInfo? idPropertyInfo = entity.GetType().GetProperty("ID");
            if (idPropertyInfo == null)
                return new DataResponse<TOutput>
                {
                    Status = DataResponseStatus.Failed,
                    Message = "Property named 'ID' not found"
                };

            //object? id = idPropertyInfo.GetValue(entity, null);
            //if (id is string)
            //    idPropertyInfo.SetValue(entity, ShortUID.Generate());

            _dbSet.Add(entity);

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("SaveChanges")!;
            int affected = (int)saveChangesMethodInfo.Invoke(_context, null)!;

            if (affected == 0)
                return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

            return new DataResponse<TOutput>
            {
                Status = DataResponseStatus.Success,
                Data = ToOutput(entity)
            };
        }

        public async Task<DataResponse<TOutput>> CreateAsync(TInput input)
        {
            TEntity entity = ToEntity(input);

            PropertyInfo? idPropertyInfo = entity.GetType().GetProperty("ID");
            if (idPropertyInfo == null)
                return new DataResponse<TOutput>
                {
                    Status = DataResponseStatus.Failed,
                    Message = "Property named 'ID' not found"
                };

            //object? id = idPropertyInfo.GetValue(entity, null);
            //if (id is string)
            //    idPropertyInfo.SetValue(entity, "");

            _dbSet.Add(entity);

            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("SaveChangesAsync")!;
            Task<int> resultAsync = (Task<int>)saveChangesMethodInfo.Invoke(_context, null)!;

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

        public DataResponse Update(object id, TInput input)
        {
            TEntity? entity_fromDb = _dbSet.Find(id);
            if (entity_fromDb == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };



            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("SaveChangesAsync")!;
            int affected = (int)saveChangesMethodInfo.Invoke(_context, null)!;

            if (affected == 0)
                return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

            return new DataResponse { Status = DataResponseStatus.Success };
        }

        public async Task<DataResponse> UpdateAsync(object id, TInput input)
        {
            TEntity? entity_fromDb = _dbSet.Find(id);
            if (entity_fromDb == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };



            MethodInfo saveChangesMethodInfo = _contextType.GetMethod("SaveChangesAsync")!;
            Task<int> resultAsync = (Task<int>)saveChangesMethodInfo.Invoke(_context, null)!;

            int affected = await resultAsync;
            if (affected == 0)
                return new DataResponse<TOutput> { Status = DataResponseStatus.Failed };

            return new DataResponse { Status = DataResponseStatus.Success };
        }


        #endregion

        #region delete records method

        public DataResponse BatchDelete(object id)
        {
            TEntity? entity = _dbSet.Find(id);
            if (entity == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };

            PropertyInfo? isDeletedPropertyInfo = entity.GetType().GetProperty("IsDeleted");
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
            TEntity? entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return new DataResponse { Status = DataResponseStatus.NotFound };

            PropertyInfo? isDeletedPropertyInfo = entity.GetType().GetProperty("IsDeleted");
            if (isDeletedPropertyInfo == null)
                throw new Exception("");

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
