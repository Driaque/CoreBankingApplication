using Core;

namespace Map
{
    public class UserMap : EntityMap<User>
    {
        public UserMap()
        {
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

            Table("Users");
        }
    }
}
