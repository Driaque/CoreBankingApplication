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
    public class SavingAccountConfigController : Controller
    {
        // GET: SavingAccountConfig
        public ActionResult Index()
        {
            var configs = new List<SavingAccountConfig>();

            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    configs = session.Query<SavingAccountConfig>().ToList();
                }
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(configs);
        }


        public ActionResult Create()
        {
            var glAccountRepository = new Repository<GLAccount>();
            ViewBag.Accounts = glAccountRepository.GetAll();
            return View(new SavingAccountConfig());
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(SavingAccountConfig savingConfig)
        {
            var glAccountRepository = new Repository<GLAccount>();
            ViewBag.Accounts = glAccountRepository.GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        savingConfig.DateAdded = DateTime.Now;
                        savingConfig.DateUpdated = DateTime.Now;
                        session.Save(savingConfig);
                        transaction.Commit();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {

            }
            return View();
        }

        public ActionResult Edit(int Id)
        {
            var glAccountRepository = new Repository<GLAccount>();
            ViewBag.Accounts = glAccountRepository.GetAll();
            using (ISession session = NHibernateHelper.Session)
            {
                var config = session.Get<SavingAccountConfig>(Id);
                return View(config);
            }
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int Id, SavingAccountConfig savingConfig)
        {
            var glAccountRepository = new Repository<GLAccount>();
            ViewBag.Accounts = glAccountRepository.GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    var updateConfig = session.Get<SavingAccountConfig>(Id);
                    var oldentity = session.Get<SavingAccountConfig>(Id);
                    updateConfig.CreditInterestRate = savingConfig.CreditInterestRate;
                    updateConfig.InterestExpenseGL = savingConfig.InterestExpenseGL;
                    updateConfig.MinimumBalance = savingConfig.MinimumBalance;
                    updateConfig.CustomerSavingAccount = savingConfig.CustomerSavingAccount;
                    using (ITransaction transaction = session.BeginTransaction())
                    {

                        updateConfig.DateAdded = oldentity.DateAdded;
                        updateConfig.DateUpdated = DateTime.Now;
                        session.Save(updateConfig);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                //return View();
            }
            return View();

        }
    }
}