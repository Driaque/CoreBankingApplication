using Core;

namespace Map
{
    public class GLCategoryMap : EntityMap<GLCategory>
    {
        public GLCategoryMap()
        {

            Map(x => x.CategoryType);
            Map(x => x.Description);
            Map(x => x.GLCategoryName);

        }

    }
}
