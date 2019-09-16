using Core;
using Data.Repository;
using Logic;
using System;
using Trx.Messaging.Iso8583;

namespace Processor
{
    public class Processor
    {
        UnitOfWork unitOfwork = new UnitOfWork();
        public Iso8583Message Process(Iso8583Message message)
        {
            if (ValidateCard(message))
            {
                //preocess message

                return new CardPostLogic().CardPosting(message);
            }
            else
            {
                //return
                return new CardPostLogic().SetResponseMessage(message, "14");
            }
        }

        public bool ValidateCard(Iso8583Message message)
        {
            string cardPan = message.Fields[2].Value.ToString();
            string accountNumber = message.Fields[102].Value.ToString();

            Card card = new Card();
            if (CheckIfCardExists(cardPan, out card) == false)
            {
                return false;
            }

            if (CheckIfAccountNumberMatches(cardPan, accountNumber, out card) == false)
            {
                return false;
            }
            return true;
        }

        public bool CheckIfCardExists(string cardPan, out Card card)
        {
            CardLogic cardLogic = new CardLogic();
            return cardLogic.CheckCard(cardPan, out card);
        }

        public bool CheckIfAccountNumberMatches(string cardPan, string accountNumber, out Card card)
        {
            CardLogic cardLogic = new CardLogic();
            return cardLogic.CheckAccountNumber(cardPan, accountNumber, out card);
        }

        public void LogTransaction(Iso8583Message message)
        {
            TransactionLog transactionLog = new TransactionLog();
            transactionLog.CardPan = message.Fields[2].Value.ToString();
            if (message.IsRequest())
            {
                transactionLog.ResponseCode = "";
            }
            else
            {
                transactionLog.ResponseCode = message.Fields[39].Value.ToString();

            }
            transactionLog.MTI = message.MessageTypeIdentifier.ToString();

            transactionLog.Amount = Convert.ToDecimal(message.Fields[4].Value);
            transactionLog.Account1 = message.Fields[102].Value.ToString();
            if (message.Fields[103] != null)
            {
                transactionLog.Account2 = message.Fields[103].Value.ToString();
            }
            transactionLog.STAN = message.Fields[11].Value.ToString();
            transactionLog.TransactionDate = DateTime.Now;
            //transactionLog.OriginalDataElement = message.Fields[90].Value.ToString();
            //transactionLog.ResponseDescription = message.Fields[39].Value.ToString();

            //EntityLogic<TransactionLog> LogLogic = new EntityLogic<TransactionLog>();
            //LogLogic.Insert(transactionLog);
            //LogLogic.Commit();
            var LogLogic = unitOfwork.EntityRepository<TransactionLog>();
            LogLogic.Save(transactionLog);


        }
    }
}
