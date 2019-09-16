using Core;
using FluentNHibernate.Mapping;

namespace Map
{
    public class EODMap : ClassMap<EOD>
    {
        public EODMap()
        {
            Id(x => x.Id);

            Map(x => x.FinancialDate);
            Map(x => x.IsOpen);

        }
    }
}
