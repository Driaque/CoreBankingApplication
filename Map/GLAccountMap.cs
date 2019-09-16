using Core;

namespace Map
{
    public class GLAccountMap : EntityMap<GLAccount>
    {
        public GLAccountMap()
        {
            Map(x => x.GLAccountName);
            References(x => x.Branch).Not.LazyLoad();
            References(x => x.GLCategory).Not.LazyLoad();
            Map(x => x.GLCode);
            Map(x => x.AccountBalance);
            Map(x => x.IsAssigned);
            Table("GLAccount");
        }
    }
}
