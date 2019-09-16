namespace Core
{
    public class ATMTerminal : Entity
    {
        public virtual string Name { get; set; }
        public virtual string TerminalID { get; set; }
        public virtual string Location { get; set; }
        public virtual GLAccount ATMTill { get; set; }
    }
}
