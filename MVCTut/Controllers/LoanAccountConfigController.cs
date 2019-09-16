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
    public class LoanAccountConfigController : Controller
    {
        public ActionResult Index()
        {
            var configs = new List<LoanAccountConfig>();

            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    configs = session.Query<LoanAccountConfig>().ToList();
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
            return View(new LoanAccountConfig());
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(LoanAccountConfig loanConfig)
        {
            var glAccountRepository = new Repository<GLAccount>();
            ViewBag.Accounts = glAccountRepository.GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        loanConfig.DateAdded = DateTime.Now;
                        loanConfig.DateUpdated = DateTime.Now;
                        session.Save(loanConfig);
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
                var config = session.Get<LoanAccountConfig>(Id);
                return View(config);
            }
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int Id, LoanAccountConfig loanConfig)
        {
            var glAccountRepository = new Repository<GLAccount>();
            ViewBag.Accounts = glAccountRepository.GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    var updateConfig = session.Get<LoanAccountConfig>(Id);
                    var oldentity = session.Get<LoanAccountConfig>(Id);
                    updateConfig.InterestIncomeGL = loanConfig.InterestIncomeGL;
                    updateConfig.DebitInterestRate = loanConfig.DebitInterestRate;
                    updateConfig.LoanGL = loanConfig.LoanGL;
                    updateConfig.DateAdded = oldentity.DateAdded;
                    updateConfig.DateUpdated = DateTime.Now;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
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