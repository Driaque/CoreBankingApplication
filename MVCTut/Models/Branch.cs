namespace MVCTut.Models
{
    
    public class Branch
    {
        public virtual int branchID { get; set; }
        
        public virtual string branchName { get; set; }
        public virtual string branchAddress { get; set; }
        public virtual Status branchStatus { get; set; }
    }
}