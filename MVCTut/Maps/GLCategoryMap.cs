using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
{
    public class GLCategoryMap : ClassMap<GLCategory>
    {
        public GLCategoryMap()
        {
            Id(x => x.Id);
            Map(x => x.CategoryType);
            Map(x => x.Description);
            Map(x => x.GLCategoryName);
        }

    }
}