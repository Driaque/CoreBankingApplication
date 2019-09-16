using Core;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace Data.Repository
{
    public class CustomerAccountRepository : Repository<CustomerAccount>
    {
        public void AddCustomerAccount(CustomerAccount customerAccount)
        {

            customerAccount.IsClosed = false;
            Save(customerAccount);
        }

        public IList<CustomerAccount> GetAccountsByFirstName(string accountName)
        {
            List<CustomerAccount> account = new List<CustomerAccount>();
            ISession session = NHibernateHelper.Session;
            ICriteria criteria = session.CreateCriteria(typeof(CustomerAccount));
            criteria.Add(Restrictions.Eq("AccountName", accountName));
            criteria.AddOrder(Order.Desc("ID"));
            account = criteria.List<CustomerAccount>() as List<CustomerAccount>;

            string sqlQuery = string.Format("select * from CustomerAccount where AccountName = {0} order by ID desc", accountName);
            ISQLQuery query = session.CreateSQLQuery(sqlQuery);
            account = query.List<CustomerAccount>() as List<CustomerAccount>;

            return account;
        }
    }
}
