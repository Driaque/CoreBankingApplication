using Core;

namespace Map
{
    public class TellerManagementMap : EntityMap<TellerManagement>
    {
        public TellerManagementMap()
        {

            References(x => x.TillAccount).Not.LazyLoad();
            References(x => x.User).Not.LazyLoad();
        }
    }
}
