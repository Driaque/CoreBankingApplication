namespace Core
{
    public class Branch : Entity
    {
        public virtual int branchID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual Status Status { get; set; }

    }
}
