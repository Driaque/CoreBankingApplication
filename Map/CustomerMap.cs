using Core;

namespace Map
{
    public class CustomerMap : EntityMap<Customer>
    {
        public CustomerMap()
        {

            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Gender);
            Map(x => x.Email);
            Map(x => x.Address);
            Map(x => x.PhoneNumber);
            Table("Customers");
        }

    }
}
