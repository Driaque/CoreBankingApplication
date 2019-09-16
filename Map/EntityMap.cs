using Core;
using FluentNHibernate.Mapping;

namespace Map
{
    public class EntityMap<T> : ClassMap<T> where T : Entity
    {
        public EntityMap()
        {
            Id(x => x.Id);
            Map(x => x.DateAdded);
            Map(x => x.DateUpdated);
        }
    }
}
