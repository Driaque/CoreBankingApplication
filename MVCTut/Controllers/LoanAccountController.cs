using Core;
using Data;
using Data.Repository;
using NHibernate;
using NHibernate.Linq;
using PagedList;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class LoanAccountController : Controller
    {
        LoanAccountRepository _loanAccountRepository;
        public LoanAccountController()
        {
            _loanAccountRepository = new LoanAccountRepository();
        }
        // GET: LoanAccount
        //public ActionResult Index()
        //{
        //    var loanAcc = new List<LoanAccount>();
        //    try
        //    {
        //        using (ISession session = NHibernateHelper.Session)
        //        {
        //            loanAcc = session.Query<LoanAccount>().ToList();
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        ViewBag.Exception = exception.Message;
        //    }
        //    return View(loanAcc);
        //}

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            // ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var loanAccounts = _loanAccountRepository.GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        loanAccounts = session.Query<LoanAccount>().Where(u => u.CustomerAccount.AccountName.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }

                    switch (sortOrder)
                    {
                        case "name_desc":
                            loanAccounts = loanAccounts.OrderByDescending(a => a.CustomerAccount.AccountName).ToList();
                            break;
                        case "id_desc":
                            loanAccounts = loanAccounts.OrderByDescending(a => a.LoanAccountNumber).ToList();
                            break;
                        default:
                            loanAccounts = loanAccounts.OrderByDescending(u => u.Id).ToList();
                            break;
                    }
                    int pageSize = 3;
                    int pageNumber = (page ?? 1);
                    return View(loanAccounts.ToPagedList(pageNumber, pageSize));
                }

                //return View(customers.ToList());
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new LoanAccount());
        }

        public ActionResult Create()
        {
            ViewBag.AccStatus = Enum.GetValues(typeof(Status));
            var customerAccountRepository = new Repository<CustomerAccount>();
            ViewBag.CustomerAccountName = customerAccountRepository.GetAll();
            var branchRepository = new Repository<Branch>();
            ViewBag.Branches = branchRepository.GetAll();
            return View(new LoanAccount());
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(LoanAccount loanAcc)
        {

            var customerAccountRepository = new Repository<CustomerAccount>();
            var glAccountRepository = new Repository<GLAccount>();
            var cusAcc = customerAccountRepository.GetById(loanAcc.CustomerAccount.Id);
            ViewBag.CustomerAccountName = customerAccountRepository.GetAll();
            ViewBag.AccStatus = Enum.GetValues(typeof(Status));
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var branch = session.Query<Branch>().FirstOrDefault(b => b.branchID == cusAcc.Branch.branchID);
                        cusAcc.Branch = branch;
                        loanAcc.LoanAccountNumber = GenerateAccountNumber(cusAcc, cusAcc.Customer.Id);
                        var loanConfig = session.Query<LoanAccountConfig>().FirstOrDefault();
                        var savingConfig = session.Query<SavingAccountConfig>().FirstOrDefault();
                        var currentConfig = session.Query<CurrentAccountConfig>().FirstOrDefault();
                        loanAcc.InterestAmount = InterestAmount(loanConfig, loanAcc.PrincipalAmount, loanAcc.Duration);
                        loanConfig.LoanGL.AccountBalance += loanAcc.PrincipalAmount;
                        loanAcc.CustomerAccount.AccountBalance += loanAcc.PrincipalAmount;
                        cusAcc.AccountBalance += loanAcc.PrincipalAmount;
                        if (cusAcc.AccountType == AccountType.Savings)
                        {
                            savingConfig.CustomerSavingAccount.AccountBalance += loanAcc.PrincipalAmount;
                        }
                        if (cusAcc.AccountType == AccountType.Current)
                        {
                            currentConfig.CustomerCurrentAccount.AccountBalance += loanAcc.PrincipalAmount;
                        }
                        customerAccountRepository.Update(cusAcc, cusAcc.Id);
                        loanAcc.DateAdded = DateTime.Now;
                        loanAcc.DateUpdated = DateTime.Now;
                        glAccountRepository.Update(loanConfig.LoanGL, loanConfig.LoanGL.Id);
                        glAccountRepository.Update(savingConfig.CustomerSavingAccount, savingConfig.CustomerSavingAccount.Id);
                        glAccountRepository.Update(currentConfig.CustomerCurrentAccount, currentConfig.CustomerCurrentAccount.Id);
                        session.Save(loanAcc);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }


            return View();
        }
        public ActionResult Edit(int id)
        {
            var customerAccountRepository = new Repository<CustomerAccount>();
            ViewBag.CustomerAccountName = customerAccountRepository.GetAll();
            var loanAccountRepository = new Repository<LoanAccount>();
            return View(loanAccountRepository.GetById(id));
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, LoanAccount loanAccount)
        {
            var customerAccountRepository = new Repository<CustomerAccount>();
            ViewBag.CustomerAccountName = customerAccountRepository.GetAll();
            if (ModelState.IsValid)
            {
                try
                {
                    var oldentity = _loanAccountRepository.GetById(id);
                    loanAccount.DateAdded = oldentity.DateAdded;
                    loanAccount.DateUpdated = DateTime.Now;
                    _loanAccountRepository.Update(loanAccount, loanAccount.Id);
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception;
                }
            }
            return View(loanAccount);
        }

        public double InterestAmount(LoanAccountConfig loanAccConfig, double amount, int duration)
        {
            return Math.Round((amount * loanAccConfig.DebitInterestRate * duration) / (100 * 365), 2);
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            ViewBag.AccType = Enum.GetValues(typeof(AccountType));
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        private string GenerateAccountNumber(CustomerAccount cusAcc, int Id)
        {
            StringBuilder builder = new StringBuilder();
            //var a = cusAcc.AccountType;
            Random r = new Random();

            int loanKey = 3;
            builder = builder.Append(loanKey);


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