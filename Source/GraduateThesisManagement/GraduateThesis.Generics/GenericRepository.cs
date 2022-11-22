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
    public class GenericRepository<TEntity, TInput, TOutput>
        where TEntity : class
        where TInput : class
        where TOutput : class
    {
        private DbSet<TEntity> _dbSet;
        private Expression<Func<TEntity, object>>[] _navigationPropertyPaths = default!;

        public Expression<Func<TEntity, TOutput>> Selector { get; set; } = default!;

        public GenericRepository(DbSet<TEntity> dbSet)
        {
            _dbSet = dbSet;
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



        #endregion

        #region update records method

       

        #endregion

        #region delete records method

        

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
