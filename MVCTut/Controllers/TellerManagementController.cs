using Core;
using Data;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class TellerManagementController : Controller
    {

        public ActionResult Index()
        {
            var tellers = new List<TellerManagement>();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    tellers = session.Query<TellerManagement>().ToList();
                }
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }
            return View(tellers);
        }

        public ActionResult Create()
        {
            var glCategoryRepository = new Repository<GLCategory>();
            ViewBag.GLtype = glCategoryRepository.GetAll();
            var userRepository = new Repository<User>();
            ViewBag.UserName = userRepository.GetAll().Where(g => !g.IsAssigned);
            var glAccountRepository = new Repository<GLAccount>();
            ViewBag.GLAccount = glAccountRepository.GetAll().Where(glAcc => !glAcc.IsAssigned && glAcc.GLCategory.Id == cash_asset_category);
            return View(new TellerManagement());
        }

        [HttpPost]
        public ActionResult Create(TellerManagement teller)
        {
            var userRepository = new Repository<User>();
            ViewBag.UserName = userRepository.GetAll();
            var glAccountRepository = new Repository<GLAccount>();
            ViewBag.GLAccount = glAccountRepository.GetAll();
            var glCategoryRepository = new Repository<GLCategory>();
            ViewBag.GLtype = glCategoryRepository.GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var user = session.Query<User>().FirstOrDefault(b => b.Id == teller.User.Id);
                        teller.User = user;

                        var till = session.Query<GLAccount>().FirstOrDefault(b => b.Id == teller.TillAccount.Id);
                        teller.TillAccount = till;

                        if (!IsCashAsset(till))
                        {
                            ModelState.AddModelError("TillAccount", "Till Account must be a Cash Asset");
                        }
                        till.IsAssigned = true;
                        user.IsAssigned = true;
                        teller.DateAdded = DateTime.Now;
                        teller.DateUpdated = DateTime.Now;
                        session.Save(teller);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            //ViewBag.GLtype = Enum.GetValues(typeof(GLCategoryType));
            return View();
        }

        private bool IsCashAsset(GLAccount glAccount)
        {
            var a = glAccount.GLCategory.Id;
            return (a == 1);
        }

        #region
        int cash_asset_category = 1;
        #endregion
    }
}