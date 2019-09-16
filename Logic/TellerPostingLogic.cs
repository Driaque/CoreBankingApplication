using Core;
using Data;
using Data.Repository;

namespace Logic
{
    public class TellerPostingLogic : Repository<TellerPosting>
    {
        UnitOfWork unitOfwork = new UnitOfWork();
        //public decimal PostTransaction(decimal Amount, CustomerAccount Account, decimal AccountBalance, GLAccount TillAccount)
        //{
        //    EntityLogic<Teller> GLlogic = new EntityLogic<Teller>();
        //    EntityLogic<User> ulogic = new EntityLogic<User>();
        //    int id = TillAccount.ID;
        //    int id2 = Account.ID;
        //    GLlogic.GetByID(id);
        //    ulogic.GetByID(id);
        TellerPosting post = new TellerPosting();

        public bool PostTransaction(TellerPosting post, out string message)
        {

            EOD EOD = new EOD();
            var EODlogic = unitOfwork.EntityRepository<EOD>();
            FinancialDate currentDate = unitOfwork.EntityRepository<FinancialDate>().GetById(1);
            // if (post.CustomerAccount.AccountStatus == Status.Active)
            if (post.CustomerAccount.IsClosed == false)
            {
                if (post.PostAmount >= 100)
                {
                    switch (post.PostType)
                    {
                        case PostType.Deposit:
                            post.Till.TillAccount.AccountBalance += post.PostAmount;
                            post.CustomerAccount.AccountBalance += post.PostAmount;
                            message = "Deposit Successful";
                            return true;

                        case PostType.Withdrawal:
                            double cot = 0;
                            if (CheckMinimumBalanceConfig(post.CustomerAccount.AccountBalance, post.PostAmount + cot) == true)
                            {
                                //NO COT
                                if (post.CustomerAccount.AccountType == AccountType.Savings)
                                {
                                    post.Till.TillAccount.AccountBalance -= post.PostAmount;
                                    post.CustomerAccount.AccountBalance -= post.PostAmount;
                                }
                                else if (post.CustomerAccount.AccountType == AccountType.Current)
                                {
                                    var currentaccount = unitOfwork.EntityRepository<CurrentAccountConfig>().GetById(1);
                                    cot = (currentaccount.CreditInterestRate) * post.PostAmount;

                                    if (post.CustomerAccount.AccountBalance > (post.PostAmount + cot))
                                    {
                                        post.Till.TillAccount.AccountBalance -= post.PostAmount;
                                        post.CustomerAccount.AccountBalance -= (post.PostAmount + cot);
                                        //post.CustomerAccount.AccountBalance -= cot;
                                        post.TransactionDate = currentDate.CurrentFinancialDate;
                                        currentaccount.COTIncomeGL.AccountBalance += cot;


                                        //:::::**IMPORTANT**:::::
                                        // EOD.Amount = cot;
                                        // EOD.CustomerAccount = post.CustomerAccount;
                                        // EOD.GLAccount = currentaccount.COTIncomeGL;
                                        // EOD.FinancialDate = currentDate.CurrentFinancialDate;

                                        EODlogic.Save(EOD);
                                    }
                                    else
                                    {
                                        //Insufficient Funds
                                        message = "Insufficient Funds";
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                message = "Insuffient funds (Minimum balance)";
                                return false;
                            }
                            message = "Transaction done";
                            return true;

                        default:
                            message = "Invalid Transaction";
                            return false;
                    }
                }
                else
                {
                    message = "Invalid amount";
                    return false;
                }
            }
            else
            {
                message = "CustomerAccount is closed";
                return false;
            }
        }

        public bool CheckMinimumBalanceConfig(double presentAccountBalance, double amount)
        {
            double remainingBalance = presentAccountBalance - amount;
            CustomerAccount customeraccount = new CustomerAccount();
            double minimumBalance = 0;
            if (customeraccount.AccountType == AccountType.Savings)
            {
                minimumBalance = unitOfwork.EntityRepository<SavingAccountConfig>().GetById(1).MinimumBalance;
            }
            else if (customeraccount.AccountType == AccountType.Current)
            {
                //minimumBalance = new EntityLogic<CurrentAccount>().GetByID(1).MinimumAmount;
                minimumBalance = unitOfwork.EntityRepository<CurrentAccountConfig>().GetById(1).MinimumBalance;
            }

            if (remainingBalance >= minimumBalance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
