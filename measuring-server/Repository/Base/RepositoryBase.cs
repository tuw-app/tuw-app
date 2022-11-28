using System.Linq.Expressions;
using System.Linq;
using System;
using MeasuringServer.Model;
using Microsoft.EntityFrameworkCore;

namespace MeasuringServer.Repository.Base
{
    public abstract class RepositoryBase<T> where T : class
    {
        private bool disposed = false;

        protected MDContext MDContext { get; set; }

        public RepositoryBase(MDContext repositoryContext)
        {
            MDContext = repositoryContext;
        }
        public IQueryable<T> FindAll()
        {
            return MDContext.Set<T>();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return MDContext.Set<T>()
                .Where(expression).AsNoTracking();
        }
        public void Create(T entity)
        {
            try
            {
                Console.WriteLine($"RepostitoryBase->Create {entity}");
                MDContext.Set<T>().Add(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Update(int id, T entity)
        {
            MDContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            MDContext.Set<T>().Remove(entity);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    MDContext.Dispose();
                }
            }
            disposed = true;
        }
    }
}
