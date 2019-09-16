using Core;

namespace Map
{
    public class TransactionLogMap : EntityMap<TransactionLog>
    {
        public TransactionLogMap()
        {
            Map(x => x.MTI);
            Map(x => x.CardPan);
            Map(x => x.Amount);
            Map(x => x.STAN);
            Map(x => x.ResponseCode);
            Map(x => x.Account1);
            Map(x => x.Account2);
            Map(x => x.TransactionDate);
            Map(x => x.ODE);
            Map(x => x.TransactionType);

        }
    }
}
