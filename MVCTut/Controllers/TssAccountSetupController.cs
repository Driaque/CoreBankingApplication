using Core;
using Data.Repository;
using System;
using System.Text;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class TssAccountSetupController : Controller
    {
        UnitOfWork unitofwork = new UnitOfWork();
        // GET: TssAccountSetup
        public ActionResult Index()
        {
            var setups = unitofwork.EntityRepository<TssAccount>().GetAll();
            try
            {
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(setups);
        }

        public ActionResult Create()
        {
            ViewBag.Catergories = unitofwork.EntityRepository<GLCategory>().GetAll();
            ViewBag.Branches = unitofwork.EntityRepository<Branch>().GetAll();
            return View();
        }

        // POST: OnUsWithdrawal/Create
        [HttpPost]
        public ActionResult Create(TssAccount tssSetup)
        {
            ViewBag.Catergories = unitofwork.EntityRepository<GLCategory>().GetAll();
            ViewBag.Branches = unitofwork.EntityRepository<Branch>().GetAll();
            try
            {
                // TODO: Add insert logic here
                var setups = unitofwork.EntityRepository<TssAccount>().Filter(c => c.Name == tssSetup.Name ||
                c.GlAccount.GLAccountName == tssSetup.GlAccount.GLAccountName);

                if (setups.Count == 0)
                {
                    tssSetup.GlAccount.GLCode = GenerateGLAccountCode(tssSetup.GlAccount, tssSetup.GlAccount.Id);
                    tssSetup.DateAdded = DateTime.Now;
                    tssSetup.DateUpdated = DateTime.Now;
                    unitofwork.EntityRepository<GLAccount>().Save(tssSetup.GlAccount);
                    unitofwork.EntityRepository<TssAccount>().Save(tssSetup);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Name", " Name exists.");
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        private string GenerateGLAccountCode(GLAccount glCode, int Id)
        {
            StringBuilder builder = new StringBuilder();
            var a = glCode.GLCategory.CategoryType;
            Random r = new Random();
            switch (a)
            {
                case GLCategoryType.Asset:
                    int assetKey = 1;
                    builder = builder.Append(assetKey);
                    break;
                case GLCategoryType.Liability:
                    int liabilityKey = 2;
                    builder = builder.Append(liabilityKey);
                    break;
                case GLCategoryType.Capital:
                    int capKey = 3;
                    builder = builder.Append(capKey);
                    break;
                case GLCategoryType.Income:
                    int IncKey = 4;
                    builder = builder.Append(IncKey);
                    break;
                case GLCategoryType.Expense:
                    int expKey = 5;
                    builder = builder.Append(expKey);
                    break;
            }
            builder.Append(Id).Append(0);
            int length = 10 - builder.Length;
            for (int i = 0; i < length; i++)
            {
                builder.Append(r.Next(0, 9));
            }
            return builder.ToString();
        }

    }
}