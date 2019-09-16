using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Id(x => x.Id);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Gender);
            Map(x => x.Email);
            Map(x => x.Address);
            Map(x => x.PhoneNumber);
            //HasMany<CustomerAccount>(x => x).Not.LazyLoad();
            Table("Customers");
        }

    }
}