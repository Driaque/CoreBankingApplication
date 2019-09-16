using FluentNHibernate.Mapping;
using MVCTut.Models;

namespace MVCTut.Maps
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