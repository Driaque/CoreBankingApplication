using Core;
using Data.Repository;
using System.Linq;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class TrialBalanceController : Controller
    {
        private GLPostingRepository _glpostingRepository;
        //public HttpSessionState session { get; set; }
        public TrialBalanceController()
        {
            _glpostingRepository = new GLPostingRepository();
        }
        // GET: TrialBalance
        public ActionResult Index()
        {
            var m = _glpostingRepository.GetAll()
                   .GroupBy(i => new { i.GLAccountToDebit, i.GLAccountToCredit, i.PostAmount })
                   .Select(i => new TrialBalance
                   {
                       Total = i.Sum(j => j.PostAmount),
                       GLAccountDebit = i.Key.GLAccountToDebit.GLAccountName.ToString(),
                       GLAccountCredit = i.Key.GLAccountToCredit.GLAccountName.ToString(),
                       CreditPost = i.Key.PostAmount.ToString()
                   }).ToList();
            return View(m);
        }
    }
}