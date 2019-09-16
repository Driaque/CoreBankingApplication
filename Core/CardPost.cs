using System;

namespace Core
{
    public class CardPost : Entity
    {
        public virtual string CardPan { get; set; }
        public virtual TransactionType TransactionType { get; set; }
        public virtual double Amount { get; set; }
        public virtual GLAccount TillAccount { get; set; }
        public virtual CustomerAccount CustomerAccount { get; set; }
        public virtual DateTime FinancialDate { get; set; }
        public virtual CustomerAccount Account2 { get; set; }
        public virtual string OriginalDataElement { get; set; }
        public virtual bool IsReversed { get; set; }
    }
}
