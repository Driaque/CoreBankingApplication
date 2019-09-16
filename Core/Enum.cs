namespace Core
{
    public enum Gender
    {
        Male, Female
    }
    public enum Status
    {
        Active, Inactive
    }
    public enum AccountType
    {
        Savings,
        Current,

    }
    public enum PostType
    {
        Deposit,
        Withdrawal
    }
    public enum UserLevel
    {
        Administrator,
        Teller
    }
    public enum GLCategoryType
    {
        Asset,
        Liability,
        Expense,
        Income,
        Capital
    }
    public enum TransactionType
    {
        BalanceEnquiry,
        Withdrawal,
        Payment,
        Reversal,
        Transfer
    }
}
