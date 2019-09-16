using Core;

namespace Map
{
    public class BranchMap : EntityMap<Branch>
    {
        public BranchMap()
        {
            Id(x => x.branchID);
            Map(x => x.Name);
            Map(x => x.Address);
            Map(x => x.Status);
            Table("Branch");
        }
    }
}
