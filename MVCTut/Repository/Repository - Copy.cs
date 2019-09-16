using Data;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MVCTut
{
    public class RepositoryTest<T> : IDisposable
    {
        public ISession Session
        {
            get
            {
                return NHibernateHelper.Session;
            }
        }

        public T GetById(int id)
        {

            return Session.Get<T>(id);
        }

        public void Save(T entity)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Session.Save(entity);
                transaction.Commit();
            }


        }

        public void Update(T entity, int id)
        {

            using (ITransaction transaction = Session.BeginTransaction())
            {
                Session.Update(entity, id);
                transaction.Commit();
            }

        }

        public List<T> GetAll()
        {

            return Session.Query<T>().ToList();

        }

        public void Delete(T entity)
        {

            using (ITransaction transaction = Session.BeginTransaction())
            {
                Session.Delete(entity);
                transaction.Commit();
            }

        }

        public List<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return Session.Query<T>().Where(predicate).ToList();

        }

        public void Dispose()
        {
            Session.Dispose();

        }
    }
}