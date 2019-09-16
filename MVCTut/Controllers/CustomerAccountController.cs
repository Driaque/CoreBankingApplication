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
    public class CustomerAccountController : Controller
    {
        private CustomerAccountRepository _customerAccountRepository;
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: CustomerAccount
        public CustomerAccountController()
        {
            _customerAccountRepository = new CustomerAccountRepository();
        }
        ISession session = NHibernateHelper.Session;
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var customerAccounts = unitOfWork.CustomerAccountRepo.GetAll(); //_customerAccountRepository.GetAll();
            var customers = unitOfWork.CustomerRepo.GetAll();
            try
            {
                return SearchCustomerAccounts(sortOrder, searchString, page, ref customerAccounts);

                //return View(customers.ToList());
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new CustomerAccount());
        }

        private ActionResult SearchCustomerAccounts(string sortOrder, string searchString, int? page, ref System.Collections.Generic.List<CustomerAccount> customerAccounts)
        {
            using (ISession session = NHibernateHelper.Session)
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    customerAccounts = session.Query<CustomerAccount>().Where(u => u.AccountName.ToUpper().Contains(searchString.ToUpper())).ToList();
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        customerAccounts = customerAccounts.OrderByDescending(a => a.AccountName).ToList();
                        break;
                    case "id_desc":
                        customerAccounts = customerAccounts.OrderByDescending(a => a.AccountNumber).ToList();
                        break;
                    default:
                        customerAccounts = customerAccounts.OrderByDescending(u => u.Id).ToList();
                        break;
                }
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(customerAccounts.ToPagedList(pageNumber, pageSize));

            }
        }

        public ActionResult Create()
        {
            ViewBag.AccStatus = Enum.GetValues(typeof(Status));
            ViewBag.Acctype = Enum.GetValues(typeof(AccountType));
            var customerRepository = new Repository<Customer>();
            ViewBag.CustomerName = customerRepository.GetAll();
            var branchRepository = new Repository<Branch>();
            ViewBag.Branches = branchRepository.GetAll();
            return View(new CustomerAccount());
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(CustomerAccount cusAcc)
        {
            var customerRepository = new Repository<Customer>();
            ViewBag.CustomerName = customerRepository.GetAll();
            ViewBag.AccStatus = Enum.GetValues(typeof(Status));
            ViewBag.Acctype = Enum.GetValues(typeof(AccountType));
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var branch = session.Query<Branch>().FirstOrDefault(b => b.branchID == cusAcc.Branch.branchID);
                        cusAcc.Branch = branch;
                        var customerdetails = session.Query<Customer>().FirstOrDefault(b => b.Id == cusAcc.Customer.Id);
                        cusAcc.Customer = customerdetails;
                        cusAcc.AccountNumber = GenerateAccountNumber(cusAcc, cusAcc.Customer.Id);
                        cusAcc.DateAdded = DateTime.Now;
                        cusAcc.DateUpdated = DateTime.Now;
                        cusAcc.IsClosed = false;
                        session.Save(cusAcc);

                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            ViewBag.AccType = Enum.GetValues(typeof(AccountType));
            return View();
        }
        [HttpGet]
        public ActionResult ChangeStatus(int? id)
        {
            var customerAccountRepository = new Repository<CustomerAccount>();
            IRepository<CustomerAccount> dao = new Repository<CustomerAccount>();

           
            CustomerAccount act = dao.GetById((int)id);
            if (act.AccountStatus == Status.Active.ToString())
            {
                act.AccountStatus = Status.Inactive.ToString();
                act.IsClosed = true;

            }
            else
            {
                act.AccountStatus = Status.Active.ToString();
                act.IsClosed = false;
            }
            customerAccountRepository.Update(act, act.Id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CustomerAccountSearch(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var customerAccounts = _customerAccountRepository.GetAll();
            try
            {
                return SearchCustomerAccounts(sortOrder, searchString, page, ref customerAccounts);

                //return View(customers.ToList());
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new CustomerAccount());
        }

        [HttpGet]
        public ActionResult Select(int id)
        {
            TempData["CustomerAccount"] = _customerAccountRepository.GetById(id);
            return RedirectToAction("Create", "TellerPosting");
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Genders = Enum.GetValues(typeof(Gender));
            ViewBag.AccType = Enum.GetValues(typeof(AccountType));
            ViewBag.AccStatus = Enum.GetValues(typeof(Status));
            var branchRepository = new Repository<Branch>();
            ViewBag.Branches = branchRepository.GetAll();

            // ViewBag.Branches = branchRepository.GetAll();
            var customerAccountRepository = new Repository<CustomerAccount>();
            return View(customerAccountRepository.GetById(id));
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CustomerAccount customerAccount)
        {
            ViewBag.Genders = Enum.GetValues(typeof(Gender));
            ViewBag.AccType = Enum.GetValues(typeof(AccountType));
            ViewBag.AccStatus = Enum.GetValues(typeof(Status));
            var branchRepository = new Repository<Branch>();
            ViewBag.Branches = branchRepository.GetAll();
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = session.Query<Customer>().FirstOrDefault(b => b.Id == customerAccount.Customer.Id);
                    customerAccount.Customer.Id = customer.Id;
                    var branch = session.Query<Branch>().FirstOrDefault(b => b.branchID == customerAccount.Branch.branchID);
                    customerAccount.Branch = branch;
                    var customerAccountRepository = new Repository<CustomerAccount>();
                    var oldentity = customerAccountRepository.GetById(id);
                    customerAccount.DateAdded = oldentity.DateAdded;
                    customerAccount.DateUpdated = DateTime.Now;
                    customerAccountRepository.Update(customerAccount, customerAccount.Id);
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception;
                }
            }



            return View(customerAccount);
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
            var a = cusAcc.AccountType;
            Random r = new Random();
            switch (a)
            {
                case AccountType.Savings:
                    int savingKey = 1;
                    builder = builder.Append(savingKey);
                    break;
                case AccountType.Current:
                    int currentKey = 2;
                    builder = builder.Append(currentKey);
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
        public int Randomize()
        {
            int randomNo;
            Random r = new Random();
            randomNo = r.Next(-100000000, 100000000);
            return randomNo;
            
        }
        public int Add(string txtbox1,  string txtbox2)
        {
            int a = Convert.ToInt32(txtbox1);
            int b = Convert.ToInt32(txtbox2);
            return (a+b);
        }


    }
}