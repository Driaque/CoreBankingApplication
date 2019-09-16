namespace Core
{
    public class Card : Entity
    {
        public virtual string PAN { get; set; }
        public virtual string ExpiryDate { get; set; }
        public virtual string CVV2 { get; set; }
        public virtual CustomerAccount Customeraccount { get; set; }
    }
}
