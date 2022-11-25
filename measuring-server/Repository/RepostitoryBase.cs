using System.Linq.Expressions;
using System.Linq;
using System;
using MeasuringServer.Model;
using Microsoft.EntityFrameworkCore;

namespace MeasuringServer.Repository
{
    public abstract class RepostitoryBase<T> where T : class
    {
        protected MDContext RepositoryContext { get; set; }

        public RepostitoryBase(MDContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }
        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>()
                .Where(expression).AsNoTracking();            
        }
        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }
    }
}
