using System;

namespace Core
{
    public class TransactionLog : Entity
    {
        public virtual string CardPan { get; set; }
        public virtual string MTI { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual string STAN { get; set; }
        public virtual DateTime TransactionDate { get; set; }
        public virtual string Account1 { get; set; }
        public virtual string Account2 { get; set; }
        public virtual string ResponseCode { get; set; }
        public virtual string ODE { get; set; }
        public virtual TransactionType TransactionType { get; set; }
    }
}
