using FluentNHibernate.Mapping;
using MVCTut.Core;

namespace MVCTut.Map
{
    public class BranchMap : ClassMap<Branch>
    {
        public BranchMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Address);
            Map(x => x.Status);
            Map(x => x.DateAdded);
            Map(x => x.DateUpdated);
            Table("Branch");
        }
    }
}