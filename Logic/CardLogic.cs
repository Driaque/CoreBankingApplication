using Core;
using Data;
using System;

namespace Logic
{
    public class CardLogic : Repository<Card>
    {
        public string GenerateCardPAN(CustomerAccount customerAccount)
        {
            Random rand = new Random();
            string cardPan = "123456" + rand.Next(100000, 999999).ToString() + customerAccount.AccountNumber.Substring(4, 4);
            return cardPan;
        }

        public DateTime GenerateExpiryDate(DateTime Date)
        {
            DateTime date = Date.AddYears(3);
            return date;
        }

        public string GenerateCVV2()
        {
            Random rand = new Random();
            string cvv2 = rand.Next(100, 999).ToString();
            return cvv2;
        }

        public bool CheckCard(string cardPan, out Card card)
        {
            CardRepository cardDAO = new CardRepository();
            return cardDAO.CheckCard(cardPan, out card);
        }

        public bool CheckAccountNumber(string cardPan, string accountNumber, out Card card)
        {
            CardRepository cardDAO = new CardRepository();
            return cardDAO.CheckAccountNumber(cardPan, accountNumber, out card);
        }
    }
}
