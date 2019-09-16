using Core;
using Data.Repository;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data
{
    public class Repository<T> : IRepository<T> where T : Entity
    {


        public T GetById(int id)
        {
            using (ISession session = NHibernateHelper.Session)
            {
                return session.Get<T>(id);
            }
        }

        public void Save(T entity)
        {
            using (ISession session = NHibernateHelper.Session)
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    entity.DateAdded = DateTime.Now;
                    entity.DateUpdated = DateTime.Now;
                    session.Save(entity);
                    transaction.Commit();
                }

            }
        }

        public void Update(T entity, int id)
        {
            using (ISession session = NHibernateHelper.Session)
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    entity.DateUpdated = DateTime.Now;
                    session.Update(entity, id);
                    transaction.Commit();
                }
            }
        }

        public List<T> GetAll()
        {
            using (ISession session = NHibernateHelper.Session)
            {
                return session.Query<T>().ToList();
            }
        }

        public void Delete(T entity)
        {
            using (ISession session = NHibernateHelper.Session)
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(entity);
                    transaction.Commit();
                }
            }
        }

        public List<T> Filter(Expression<Func<T, bool>> predicate)
        {
            using (ISession session = NHibernateHelper.Session)
            {
                return session.Query<T>().Where(predicate).ToList();
            }
        }


    }
}
