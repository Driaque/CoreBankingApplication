using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class GLAccountMap : ClassMap<GLAccount>
    {
        public GLAccountMap()
        {
            Id(x => x.Id);
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