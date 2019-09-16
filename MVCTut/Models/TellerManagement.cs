namespace MVCTut.Models
{
    public class TellerManagement
    {
        public virtual int Id { get; set; }
        public virtual User User { get; set; }
        public virtual GLAccount TillAccount { get; set; }
    }
}