using FluentNHibernate.Mapping;
using MVCTut.Core;

namespace MVCTut.Map
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.FullName);
            Map(x => x.Email);
            References(x => x.Branch).Not.LazyLoad();
            Map(x => x.PhoneNumber);
            Map(x => x.UserName);
            Map(x => x.Password);
            Map(x => x.IsAssigned);
            Map(x => x.IsAdmin);
            Map(x => x.DateAdded);
            Map(x => x.DateUpdated);
            Table("Users");
        }
    }
}