using Core;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repository
{
    public class UnitOfWork
    {
        ISession session = NHibernateHelper.Session;


        //Getters for each repo - meant to make this Generic

        private Repository<CustomerAccount> customeraccountRepository = null;
        private Repository<Customer> customerRepository = null;
        private Repository<Branch> branchRepository = null;

        public Dictionary<Type, object> repositoriesType = new Dictionary<Type, object>();
        public Repository<T> EntityRepository<T>() where T : Entity
        {
            //check where this T exists in repositoriestype
            //if true, return it
            //else, get it and insert into repostoriestype
            if (this.repositoriesType.Keys.Contains(typeof(T)) == true)
            {
                return this.repositoriesType[typeof(T)] as Repository<T>;
            }
            else
            {
                Repository<T> repo = new Repository<T>();
                repositoriesType.Add(typeof(T), repo);
                return repo;
            }



        }

        public Repository<CustomerAccount> CustomerAccountRepo
        {
            get
            {
                if (customeraccountRepository != null)
                    return this.customeraccountRepository;
                else
                    return this.customeraccountRepository = new Repository<CustomerAccount>();

            }
        }

        public Repository<Customer> CustomerRepo
        {
            get
            {
                if (customerRepository != null)
                    return this.customerRepository;
                else
                    return this.customerRepository = new Repository<Customer>();

            }
        }




    }
}
