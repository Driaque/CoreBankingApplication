using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class CustomerAccountMap : ClassMap<CustomerAccount>
    {
        public CustomerAccountMap()
        {
            Id(x => x.Id);
            Map(x => x.AccountNumber);
            Map(x => x.AccountName);
            References(x => x.Branch).Not.LazyLoad();
            Map(x => x.AccountType);
            Map(x => x.AccountStatus);
            Map(x => x.AccountBalance);
            Map(x => x.IsClosed);
            References(x => x.Customer).Not.LazyLoad();
            Table("CustomerAccount");
        }
    }
}