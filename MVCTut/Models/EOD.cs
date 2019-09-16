using System;

namespace MVCTut.Models
{
    public class EOD
    {
        public virtual int Id { get; set; }
        public virtual DateTime FinancialDate { get; set; }
        public virtual bool IsOpen { get; set; }
    }
}