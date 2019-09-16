
using Core;
using Data;
using NHibernate.Linq;
using System.Linq;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class ProfitLossController : Controller
    {
        // GET: FinancialReport

        public ActionResult Index()
        {
            var m = NHibernateHelper.Session.Query<GLAccount>().Where(i => i.GLCategory.CategoryType == GLCategoryType.Income || i.GLCategory.CategoryType == GLCategoryType.Expense)
                   .GroupBy(i => new { i.GLCategory.CategoryType, i.GLAccountName })
                   .Select(i => new ProfitLoss
                   {
                       Total = i.Sum(j => j.AccountBalance),
                       GLCatergoryName = i.Key.CategoryType.ToString(),
                       GLAccountName = i.Key.GLAccountName
                   }).ToList();
            return View(m);
        }


    }
}