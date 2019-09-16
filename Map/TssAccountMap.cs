using Core;

namespace Map
{
    public class TssAccountMap : EntityMap<TssAccount>
    {
        public TssAccountMap()
        {

            Map(x => x.Name);
            References(x => x.GlAccount).Not.LazyLoad();

        }
    }
}
