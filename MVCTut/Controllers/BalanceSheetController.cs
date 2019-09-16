using Core;
using Data;
using NHibernate.Linq;
using System.Linq;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class BalanceSheetController : Controller
    {
        // GET: BalanceSheet
        public ActionResult Index()
        {
            var m = NHibernateHelper.Session.Query<GLAccount>().Where(i => i.GLCategory.CategoryType == GLCategoryType.Asset ||
            i.GLCategory.CategoryType == GLCategoryType.Liability || i.GLCategory.CategoryType == GLCategoryType.Capital)
                    .GroupBy(i => new { i.GLCategory.CategoryType, i.GLAccountName })
                    .Select(i => new BalanceSheet
                    {
                        Total = i.Sum(j => j.AccountBalance),
                        GLCatergoryName = i.Key.CategoryType.ToString(),
                        GLAccountName = i.Key.GLAccountName
                    }).ToList();
            return View(m);
        }
    }
}