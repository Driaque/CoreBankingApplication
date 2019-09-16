//using Core;
//using Data;
//using Data.Repository;
//using System;
//using System.Linq;
//using Trx.Messaging.Iso8583;

//namespace Logic
//{
//    public class CardPostLogic : Repository<CardPost>
//    {
//        UnitOfWork unitOfwork = new UnitOfWork();

//        CustomerAccount Account1 = new CustomerAccount();
//        CustomerAccount Account2 = new CustomerAccount();
//        double Amount;
//        //string CardPan;
//        TransactionType type;
//        string Terminal;
//        //string responseCode = "06";
//        Iso8583Message responseMessage;
//        GLAccount till = new GLAccount();

//        TellerPostingLogic tellerPostLogic = new TellerPostingLogic();

//        public Iso8583Message CardPosting(Iso8583Message message)
//        {

//            string transactionType = message.Fields[3].Value.ToString().Substring(0, 2);
//            string account1 = message.Fields[102].Value.ToString();
//            Terminal = message.Fields[41].Value.ToString().Substring(1, message.Fields[41].Value.ToString().Length - 1);
//            Account1 = unitOfwork.EntityRepository<CustomerAccount>().GetAll().Where(x => x.AccountNumber == account1).First();

//            if (message.Fields[103] != null)
//            {
//                string account2 = message.Fields[103].Value.ToString();
//                try
//                {
//                    Account2 = unitOfwork.EntityRepository<CustomerAccount>().GetAll().Where(x => x.AccountNumber == account2).FirstOrDefault();

//                }
//                catch (Exception e)
//                {
//                    //return account2 not found
//                    throw;
//                }
//            }


//            Amount = Convert.ToDouble(message.Fields[4].Value) / 100;
//            string cardPan = message.Fields[2].Value.ToString();


//            responseMessage = message;


//            if (message.MessageTypeIdentifier.ToString() == "420")
//            {
//                string dataElement2 = message.Fields[90].Value.ToString();

//                string dataElement = "0200" + message.Fields[90].Value.ToString().Substring(4, dataElement2.Length - 4);

//                CardPost cardPost;
//                try
//                {
//                    cardPost = unitOfwork.EntityRepository<CardPost>().GetAll().Where(x => x.OriginalDataElement == dataElement).First();
//                }
//                catch (Exception e)
//                {
//                    SetResponseMessage(responseMessage, "25");
//                    return responseMessage;
//                }

//                DoReversal(cardPost);
//                return responseMessage;
//            }

//            if (transactionType == "31")
//            {
//                BalanceEnquiry(Account1);
//                type = TransactionType.BalanceEnquiry;
//            }

//            else if (transactionType == "00")
//            {
//                Withdrawal(Account1, Amount);
//                type = TransactionType.Withdrawal;
//            }

//            else if (transactionType == "01")
//            {
//                Payment(Account1, Amount);
//                type = TransactionType.Payment;
//            }


//            else if (transactionType == "50")
//            {
//                Transfer(Account1, Account2, Amount);
//                type = TransactionType.Transfer;
//            }

//            CardPost post = new CardPost();
//            EOD eod = new EOD();
//            eod = unitOfwork.EntityRepository<EOD>().GetById(1);
//            var eodFinDate = eod.FinancialDate;
//            FinancialDate currentDate = new FinancialDate();
//            currentDate.CurrentFinancialDate = eodFinDate;//unitOfwork.EntityRepository<FinancialDate>().GetById(1);

//            post.Amount = Amount;
//            post.CardPan = cardPan;
//            post.FinancialDate = currentDate.CurrentFinancialDate;
//            if (type == TransactionType.Withdrawal || type == TransactionType.Payment)
//            {
//                post.TillAccount = till;
//            }
//            post.TransactionType = type;
//            post.OriginalDataElement = message.Fields[90].Value.ToString();
//            post.CustomerAccount = Account1; //new EntityLogic<CustomerAccount>().GetAll().Where(x => x.AccountNumber == account1).First();
//            if (type == TransactionType.Transfer)
//            {
//                post.Account2 = Account2;
//            }
//            var cardPostLogic = unitOfwork.EntityRepository<CardPost>();
//            //var TransactionLogLogic = unitOfwork.EntityRepository<TransactionLog>();
//            //TransactionLog LogTransaction = new TransactionLog();
//            //TransactionLogLogic.Update(LogTransaction, LogTransaction.Id);
//            // LogTransaction.ODE = post.OriginalDataElement;

//            cardPostLogic.Save(post);
//            return responseMessage;
//        }

//        public void BalanceEnquiry(CustomerAccount account)
//        {

//            //throw new NotImplementedException();
//            //message.Fields.Add(54, account.AccountBalance.ToString());
//            responseMessage.Fields.Add(54, FormatBalanceEnquiryField(account.AccountBalance));
//            SetResponseMessage(responseMessage, "00");
//        }

//        public bool Withdrawal(CustomerAccount account, double amount)
//        {
//            till = unitOfwork.EntityRepository<ATMTerminal>().GetAll().Where(x => x.Id == 1).FirstOrDefault().ATMTill;
//            //insufficient is 51
//            if (tellerPostLogic.CheckMinimumBalanceConfig(account.AccountBalance, amount) == true)
//            {
//                account.AccountBalance -= amount;
//                try
//                {
//                    if (till.AccountBalance < amount)
//                    {
//                        //temporarily unable to dispense cash, i.e No money
//                        //end flow
//                        SetResponseMessage(responseMessage, "06");
//                        return false;
//                        //06
//                    }

//                    else
//                    {
//                        //credit an asset GL
//                        till.AccountBalance -= amount;
//                        var customerAccRepo = unitOfwork.EntityRepository<CustomerAccount>();
//                        customerAccRepo.Update(account, account.Id);
//                        var tillAccRepo = unitOfwork.EntityRepository<GLAccount>();
//                        tillAccRepo.Update(till, till.Id);
//                        SetResponseMessage(responseMessage, "00");
//                        return true;
//                    }

//                }
//                catch (Exception ex)
//                {
//                    //something went wrong
//                    throw;
//                }
//            }
//            //responseCode 51
//            SetResponseMessage(responseMessage, "51");
//            return false;
//        }

//        public bool Payment(CustomerAccount account, double amount)
//        {
//            if (tellerPostLogic.CheckMinimumBalanceConfig(account.AccountBalance, amount) == true)
//            {
//                account.AccountBalance -= amount;
//                try
//                {
//                    till = unitOfwork.EntityRepository<ATMTerminal>().GetAll().Where(x => x.Id == 1).FirstOrDefault().ATMTill;
//                    //credit an asset GL
//                    till.AccountBalance -= amount;
//                    var customerAccRepo = unitOfwork.EntityRepository<CustomerAccount>();
//                    customerAccRepo.Update(account, account.Id);
//                    var tillAccRepo = unitOfwork.EntityRepository<GLAccount>();
//                    tillAccRepo.Update(till, till.Id);
//                    SetResponseMessage(responseMessage, "00");
//                    return true;
//                }
//                catch (Exception ex)
//                {
//                    //something went wrong
//                    throw;
//                }
//            }
//            //responseCode 51
//            SetResponseMessage(responseMessage, "51");
//            return false;

//        }

//        public bool Transfer(CustomerAccount fromAccount, CustomerAccount toAccount, double amount)
//        {
//            if (tellerPostLogic.CheckMinimumBalanceConfig(fromAccount.AccountBalance, amount) == true)
//            {
//                fromAccount.AccountBalance -= amount;
//                toAccount.AccountBalance += amount;
//                var customerAccRepo = unitOfwork.EntityRepository<CustomerAccount>();
//                customerAccRepo.Update(fromAccount, fromAccount.Id);
//                customerAccRepo.Update(toAccount, toAccount.Id);

//                SetResponseMessage(responseMessage, "00");
//                return true;
//            }
//            return false;
//        }

//        public Iso8583Message SetResponseMessage(Iso8583Message message, string responseCode)
//        {
//            message.SetResponseMessageTypeIdentifier();
//            message.Fields.Add(39, responseCode);
//            return message;
//        }

//        private string FormatBalanceEnquiryField(double balance)
//        {
//            Console.WriteLine("Original: " + balance);

//            balance = Math.Round(balance, 2);
//            Console.WriteLine("Round :" + balance);

//            balance = balance * 100;
//            Console.WriteLine("Kobo: " + balance);

//            balance = Math.Round(balance, 0);
//            Console.WriteLine("Round to 0:" + balance);

//            string str_balance = Convert.ToString(balance);
//            Console.WriteLine("String: " + str_balance);

//            string padded = str_balance.PadLeft(12, '0');
//            Console.WriteLine("Padded: " + padded);

//            string ledgerBalance = "00" + "01" + "566" + "C" + padded;
//            string availableBalance = "00" + "02" + "566" + "C" + padded;
//            return ledgerBalance + availableBalance;
//            //throw new NotImplementedException();
//        }

//        public void DoReversal(CardPost post)
//        {

//            if (post.Account2 != null && post.TillAccount == null)
//            {
//                //transfer
//                //call transfer reversal methood
//                TransferReversal(post.CustomerAccount, post.Account2, post.Amount);
//                post.IsReversed = true;
//            }
//            else if (post.Account2 == null && post.TillAccount != null)
//            {
//                //withdrawal
//                //call withdrawal reversal method
//                WithdrawalReversal(post.CustomerAccount, post.TillAccount, post.Amount);
//                post.IsReversed = true;
//            }



//        }

//        public void TransferReversal(CustomerAccount fromAccount, CustomerAccount toAccount, double amount)
//        {
//            fromAccount.AccountBalance += amount;
//            toAccount.AccountBalance -= amount;
//            var customerAccRepo = unitOfwork.EntityRepository<CustomerAccount>();
//            customerAccRepo.Update(fromAccount, fromAccount.Id);
//            customerAccRepo.Update(toAccount, toAccount.Id);
//            SetResponseMessage(responseMessage, "00");
//        }

//        public void WithdrawalReversal(CustomerAccount Account, GLAccount GLAcc, double amount)
//        {
//            till.AccountBalance += amount;
//            Account.AccountBalance += amount;
//            var customerAccRepo = unitOfwork.EntityRepository<CustomerAccount>();
//            customerAccRepo.Update(Account, Account.Id);
//            var tillAccRepo = unitOfwork.EntityRepository<GLAccount>();
//            tillAccRepo.Update(till, till.Id);
//            SetResponseMessage(responseMessage, "00");
//        }
//    }
//}
