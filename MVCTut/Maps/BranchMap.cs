using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class BranchMap : ClassMap<Branch>
    {
        public BranchMap()
        {
            Id(x => x.branchID);
            Map(x => x.branchName);
            Map(x => x.branchAddress);
            Map(x => x.branchStatus);
            Table("Branch");
        }
    }
}