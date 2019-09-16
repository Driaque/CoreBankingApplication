namespace Core
{
    public class TellerManagement : Entity
    {
        public virtual User User { get; set; }
        public virtual GLAccount TillAccount { get; set; }
    }
}
