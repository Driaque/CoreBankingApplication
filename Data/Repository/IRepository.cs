using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Repository
{
    public interface IRepository<T>
    {
        void Save(T entity);
        T GetById(int id);

        void Update(T entity, int id);
        List<T> GetAll();
        void Delete(T entity);
        List<T> Filter(Expression<Func<T, bool>> predicate);
    }
}
