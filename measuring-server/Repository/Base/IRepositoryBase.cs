using System.Linq.Expressions;
using System.Linq;
using System;

namespace MeasuringServer.Repository.Base
{
    public interface IRepositoryBase<T> : IDisposable
        where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        T Get(long id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
