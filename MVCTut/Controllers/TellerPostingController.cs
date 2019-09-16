using Core;
using Data;
using Data.Repository;
using MVCTut.CustomAttributes;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class TellerPostingController : Controller
    {
        private TellerPostingRepository _tellerPostingRepository;

        public TellerPostingController()
        {
            _tellerPostingRepository = new TellerPostingRepository();
        }
        // GET: TellerPosting
        public ActionResult Index()
        {
            try
            {
                return View(_tellerPostingRepository.GetAll());
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new TellerPosting());
        }

        [RestrictPostTransaction]
        public ActionResult Create()
        {
            var tellerRepository = new Repository<TellerManagement>();
            ViewBag.tillAccounts = tellerRepository.GetAll();
            var glAccountRepository = new Repository<GLAccount>();
            var customerAccountRepository = new Repository<CustomerAccount>();
            TellerPosting tellerPosting = new TellerPosting();

            tellerPosting.CustomerAccount = (CustomerAccount)TempData["CustomerAccount"];
            ViewBag.postType = Enum.GetValues(typeof(PostType));
            return View(tellerPosting);
        }


        [HttpPost]
        [RestrictPostTransaction]
        public ActionResult Create([Bind(Exclude = "Till")] TellerPosting tellerPost)
        {
            var userdetails = (User)Session["User"];
            //int userID = userdetails.Id;
            bool IsUserAssigned = userdetails.IsAssigned;

            var tellerRepository = new Repository<TellerManagement>();

            var glAccountRepository = new Repository<GLAccount>();
            var customerAccountRepository = new Repository<CustomerAccount>();
            ViewBag.postType = Enum.GetValues(typeof(PostType));

            if (IsUserAssigned)

            {

                if (ModelState.IsValid)
                {
                    try
                    {

                        using (ISession session = NHibernateHelper.Session)
                        {
                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                //get user's till account
                                var till = session.Query<TellerManagement>().FirstOrDefault(x => x.User == userdetails);
                                //For Till Account
                                var till_gl_account = session.Query<GLAccount>().FirstOrDefault(b => b.Id == till.TillAccount.Id);
                                //tellerPost.Till.TillAccount = tillAccount;
                                tellerPost.Till = till;
                                //For Customer Account
                                var customerAccount = session.Query<CustomerAccount>().FirstOrDefault(b => b.Id == tellerPost.CustomerAccount.Id);
                                tellerPost.CustomerAccount = customerAccount;
                                tellerPost.TransactionDate = DateTime.Now;
                                // SavingAccountConfig savingAccountConfig = new SavingAccountConfig();
                                //CurrentAccountConfig currentAccountConfig = new CurrentAccountConfig();

                                var current = session.Query<CurrentAccountConfig>().FirstOrDefault();
                                var savings = session.Query<SavingAccountConfig>().FirstOrDefault();



                                if (!customerAccount.IsClosed)
                                {
                                    if (IsPostDeposit(tellerPost.PostType))
                                    {
                                        till_gl_account.AccountBalance += tellerPost.PostAmount;
                                        customerAccount.AccountBalance += tellerPost.PostAmount;
                                        if (customerAccount.AccountType == AccountType.Savings)
                                        {
                                            savings.CustomerSavingAccount.AccountBalance += tellerPost.PostAmount;
                                        }
                                        if (customerAccount.AccountType == AccountType.Current && current != null)
                                        {
                                            current.CustomerCurrentAccount.AccountBalance += tellerPost.PostAmount;
                                        }
                                    }
                                    else
                                    {
                                        if (tellerPost.PostAmount <= till_gl_account.AccountBalance)
                                        {
                                            if (tellerPost.PostAmount <= customerAccount.AccountBalance)
                                            {
                                                //
                                                if (customerAccount.AccountBalance >= savings.MinimumBalance ||
                                                    customerAccount.AccountBalance >= current.MinimumBalance)
                                                {
                                                    till_gl_account.AccountBalance -= tellerPost.PostAmount;
                                                    customerAccount.AccountBalance -= tellerPost.PostAmount;

                                                    if (customerAccount.AccountType == AccountType.Savings)
                                                    {
                                                        savings.CustomerSavingAccount.AccountBalance -= tellerPost.PostAmount;
                                                    }
                                                    if (customerAccount.AccountType == AccountType.Current)
                                                    {
                                                        current.CustomerCurrentAccount.AccountBalance -= tellerPost.PostAmount;
                                                        current.CustomerCurrentAccount.AccountBalance -= current.COT;
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                ModelState.AddModelError("", "Insufficient Balance!");
                                                return View();
                                            }
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("", "Not Enough Money in Till Account");
                                            return View();
                                        }
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Cannot Perform Transactions on Closed accounts");
                                    return View();
                                }
                                glAccountRepository.Update(till_gl_account, till_gl_account.Id);
                                glAccountRepository.Update(savings.CustomerSavingAccount, savings.CustomerSavingAccount.Id);
                                glAccountRepository.Update(current.CustomerCurrentAccount, current.CustomerCurrentAccount.Id);
                                customerAccountRepository.Update(customerAccount, customerAccount.Id);
                                tellerPost.TransactionDate = DateTime.Now;
                                tellerPost.DateAdded = DateTime.Now;
                                tellerPost.DateUpdated = DateTime.Now;
                                _tellerPostingRepository.Save(tellerPost);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Exception = exception.Message;
                    }
                }

            }

            else
            {
                ModelState.AddModelError("", "User is not assinged to any Till Account");
            }
            return View(new TellerPosting());
        }

        private bool IsPostDeposit(PostType postType)
        {

            if (postType == PostType.Deposit)
            {
                return true;
            }
            return false;
            //return (a == PostType.Deposit || a == PostType.Withdrawal);
        }



    }
}