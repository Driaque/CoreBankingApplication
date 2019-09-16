namespace Core
{
    public class TssAccount : Entity
    {
        public virtual string Name { get; set; }
        public virtual GLAccount GlAccount { get; set; }
    }
}
