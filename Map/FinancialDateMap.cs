using Core;

namespace Map
{
    public class FinancialDateMap : EntityMap<FinancialDate>
    {
        public FinancialDateMap()
        {
            Map(x => x.CurrentFinancialDate);
        }
    }
}
