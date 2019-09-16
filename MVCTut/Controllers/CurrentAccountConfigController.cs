using Core;
using Data;
using Data.Repository;
using NHibernate;
using System;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class CurrentAccountConfigController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Index()
        {
            var configs = unitOfWork.EntityRepository<CurrentAccountConfig>().GetAll();

            try
            {

            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }
            return View(configs);
        }


        public ActionResult Create()
        {
            // var glAccountRepository = new Repository<GLAccount>();
            var glAccountRepository = unitOfWork.EntityRepository<GLAccount>();
            ViewBag.Accounts = glAccountRepository.GetAll();
            return View(new CurrentAccountConfig());
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(CurrentAccountConfig currentConfig)
        {
            // var glAccountRepository = new Repository<GLAccount>();
            var glAccountRepository = unitOfWork.EntityRepository<GLAccount>();
            ViewBag.Accounts = glAccountRepository.GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        currentConfig.DateAdded = DateTime.Now;
                        currentConfig.DateUpdated = DateTime.Now;
                        session.Save(currentConfig);
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
                var config = session.Get<CurrentAccountConfig>(Id);
                return View(config);
            }
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int Id, CurrentAccountConfig currentConfig)
        {
            var glAccountRepository = new Repository<GLAccount>();
            ViewBag.Accounts = glAccountRepository.GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    var updateConfig = session.Get<CurrentAccountConfig>(Id);
                    var oldentity = session.Get<LoanAccountConfig>(Id);
                    updateConfig.CreditInterestRate = currentConfig.CreditInterestRate;
                    updateConfig.InterestExpenseGL = currentConfig.InterestExpenseGL;
                    updateConfig.MinimumBalance = currentConfig.MinimumBalance;
                    updateConfig.COT = currentConfig.COT;
                    updateConfig.COTIncomeGL = currentConfig.COTIncomeGL;
                    updateConfig.CustomerCurrentAccount = currentConfig.CustomerCurrentAccount;
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