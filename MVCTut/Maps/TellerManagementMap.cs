using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class TellerManagementMap : ClassMap<TellerManagement>
    {
        public TellerManagementMap()
        {
            Id(x => x.Id);
            References(x => x.TillAccount).Not.LazyLoad();
            References(x => x.User).Not.LazyLoad();
        }
    }
}