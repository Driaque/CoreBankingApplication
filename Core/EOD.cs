using System;

namespace Core
{
    public class EOD : Entity
    {
        public virtual DateTime FinancialDate { get; set; }
        public virtual bool IsOpen { get; set; }
    }
}
