using Core;
using NHibernate;
using System.Linq;

namespace Data
{
    public class CardRepository : Repository<Card>
    {
        public bool CheckCard(string cardPan, out Card card)
        {
            using (ISession session = NHibernateHelper.Session)
            {
                card = session.QueryOver<Card>().Where(x => x.PAN == cardPan).SingleOrDefault<Card>();
            }
            if (card == null)
            {
                return false;
            }
            return true;
        }

        public bool CheckAccountNumber(string cardPan, string accountNumber, out Card card)
        {
            card = GetAll().Where(x => x.PAN == cardPan && x.Customeraccount.AccountNumber == accountNumber).First();
            //card = GetSession().QueryOver<Card>().Where(x => x.PAN == cardPan && x.Customeraccount.AccountNumber == accountNumber).fi
            if (card == null)
            {
                return false;
            }
            return true;
        }
    }
}
