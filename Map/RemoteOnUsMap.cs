using Core;

namespace Map
{
    public class RemoteOnUsMap : EntityMap<RemoteOnUs>
    {
        public RemoteOnUsMap()
        {
            Map(x => x.Name);
            References(x => x.GlAccount).Not.LazyLoad();

        }
    }
}
