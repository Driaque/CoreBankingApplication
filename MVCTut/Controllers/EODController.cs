using Core;
using Data;
using NHibernate;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class EODController : Controller
    {

        public const int numberOfDaysInAYear = 365;
        public const int numberOfDaysInAMonth = 30;
        private Repository<EOD> eodRepo = new Repository<EOD>();
        private Repository<SavingAccountConfig> savingsAccConfigRepo = new Repository<SavingAccountConfig>();
        private Repository<CurrentAccountConfig> currentAccConfigRepo = new Repository<CurrentAccountConfig>();
        private Repository<LoanAccountConfig> loanAccConfigRepo = new Repository<LoanAccountConfig>();
        public Repository<CustomerAccount> customerAccountsRepo = new Repository<CustomerAccount>();

        // GET: EOD
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Business()
        {
            Repository<EOD> eodRepo = new Repository<EOD>();
            var eod = eodRepo.GetById(1);
            ViewBag.IsOpen = eod.IsOpen;
            ViewBag.CurrentFinancialDate = eod.FinancialDate;




            return View(eod);
        }

        public ActionResult OpenBusiness()
        {
            var eod = eodRepo.GetById(1);
            if (eod != null)
            {
                if (eod.IsOpen == true)
                {
                    ViewBag.IsBusinessOpened = true;
                    ViewBag.BusinessOpenedMessage = "Business is opened already!!";
                    //MvcApplication.IsBusinessOpen = true;
                }
                else
                {
                    ViewBag.IsBusinessOpened = false;
                }
            }
            else
            {
                ViewBag.IsBusinessOpened = false;
            }

            //ViewBag.CurrentFinancialDate = DateTime.Now.ToString("dd, MMMM, yyyy");

            ViewBag.CurrentFinancialDate = eodRepo.GetById(1).FinancialDate.ToString("dd, MMMM, yyyy");
            return View(eod);
        }

        [HttpPost]
        public ActionResult OpenBusiness(int? id)
        {
            var eod = eodRepo.GetById(1);
            if (eod != null)
            {
                if (eod.IsOpen == true)
                {

                    //MvcApplication.IsBusinessOpen = true;
                }
                else
                {
                    MvcApplication.IsBusinessOpen = true;
                    ViewBag.IsBusinessOpened = true;
                    ViewBag.BusinessOpenedMessage = "Business is opened already!!";
                }
            }
            else
            {
                ViewBag.IsBusinessOpened = false;
            }
            eod.IsOpen = true;
            eodRepo.Update(eod, eod.Id);
            ViewBag.CurrentFinancialDate = eodRepo.GetById(1).FinancialDate.ToString("dd, MMMM, yyyy");

            return RedirectToAction("Business", "EOD");
        }

        [HttpGet]
        public ActionResult CloseBusiness()
        {
            //TODO: check for null pointer
            var eod = eodRepo.GetById(1);
            if (eod != null)
            {
                if (!eod.IsOpen)
                {
                    ViewBag.IsBusinessClosed = true;
                    ViewBag.BusinessClosedMessage = "Business is Closed already!!";
                }
                else
                {
                    ViewBag.IsBusinessClosed = false;
                    SavingAccountConfig savingsConfig = savingsAccConfigRepo.GetById(1);
                    CurrentAccountConfig currentAccountConfig = currentAccConfigRepo.GetById(1);
                    LoanAccountConfig loansConfig = loanAccConfigRepo.GetById(1);
                    if (savingsConfig == null || currentAccountConfig == null || loansConfig == null)
                    {
                        ViewBag.IsAllAccountsConfigured = false;
                        if (savingsConfig == null)
                            ViewBag.IsSavingsConfigured = false;
                        else
                            ViewBag.IsSavingsConfigured = true;
                        if (currentAccountConfig == null)
                            ViewBag.IsCurrentAccountConfigured = false;
                        else
                            ViewBag.IsCurrentAccountConfigured = true;
                        if (loansConfig == null)
                            ViewBag.IsLoanConfigured = false;
                        else
                            ViewBag.IsLoanConfigured = true;
                    }
                    else
                    {
                        ViewBag.IsAllAccountsConfigured = true;
                    }
                }
            }
            else
            {
                ViewBag.IsBusinessClosed = false;
            }

            ViewBag.CurrentFinancialDate = eodRepo.GetById(1).FinancialDate.ToString("dd, MMMM, yyyy");
            return View(eod);
        }

        [HttpPost]
        public ActionResult CloseBusiness(int? id)
        {
            var eod = eodRepo.GetById(1);

            if (!eod.IsOpen == true)
            {
                ViewBag.IsBusinessClosed = true;
                ViewBag.BusinessClosedMessage = "Business is Closed already!!";

            }
            else
            {
                ViewBag.IsBusinessClosed = false;
                SavingAccountConfig savingsConfig = savingsAccConfigRepo.GetById(1);//
                CurrentAccountConfig currentAccountConfig = currentAccConfigRepo.GetById(1);
                LoanAccountConfig loansConfig = loanAccConfigRepo.GetById(1);
                if (savingsConfig == null || currentAccountConfig == null || loansConfig == null)
                {
                    ViewBag.IsAllAccountsConfigured = false;
                    if (savingsConfig == null)
                        ViewBag.IsSavingsConfigured = false;
                    else
                        ViewBag.IsSavingsConfigured = true;
                    if (currentAccountConfig == null)
                        ViewBag.IsCurrentAccountConfigured = false;
                    else
                        ViewBag.IsCurrentAccountConfigured = true;
                    if (loansConfig == null)
                        ViewBag.IsLoanConfigured = false;
                    else
                        ViewBag.IsLoanConfigured = true;
                }
                else
                {
                    ViewBag.IsAllAccountsConfigured = true;
                }
                if (ViewBag.IsAllAccountsConfigured)
                {
                    RunEOD(savingsConfig, currentAccountConfig, loansConfig);
                    eod.IsOpen = false;
                    eod.FinancialDate = DateTime.Now;
                    ViewBag.CurrentFinancialDate = eod.FinancialDate.ToString("dd,MMMM,yyyy");
                    MvcApplication.IsBusinessOpen = false;
                    ViewBag.IsBusinessClosed = true;
                    ViewBag.BusinessClosedMessage = "Business was closed successfully";

                }
            }

            ViewBag.CurrentFinancialDate = eodRepo.GetById(1).FinancialDate.ToString("dd, MMMM, yyyy");
            return RedirectToAction("Business", "EOD");
            //return View();
        }

        private void RunEOD(SavingAccountConfig savingsConfig, CurrentAccountConfig currentAccountConfig, LoanAccountConfig loansConfig)
        {
            var sac = savingsAccConfigRepo.GetById(1);
            var cac = currentAccConfigRepo.GetById(1);
            var lac = loanAccConfigRepo.GetById(1);
            var cotIncomeGL = cac.COTIncomeGL;
            var currentInterestExpenseGL = cac.InterestExpenseGL;
            var savingInterestExpenseGL = sac.InterestExpenseGL;
            var loanInterestIncomeGL = lac.InterestIncomeGL;
            var glAccountRepository = new Repository<GLAccount>();
            var customerAccountRepository = new Repository<CustomerAccount>();
            var customerAccounts = customerAccountRepository.GetAll().ToList();
            var loanAccountRepository = new Repository<LoanAccount>();
            var loanAccounts = loanAccountRepository.GetAll().ToList();
            var savingGLAccount = sac.CustomerSavingAccount;
            var currentGLAccount = cac.CustomerCurrentAccount;
            var loanGLAccount = lac.LoanGL;
            using (ISession session = NHibernateHelper.Session)
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (customerAccounts == null || cotIncomeGL == null || currentInterestExpenseGL == null ||
                        savingInterestExpenseGL == null || savingGLAccount == null || currentGLAccount == null)
                    {
                        throw new Exception("Create All GL Accounts and customer accounts");
                    }

                    foreach (var accounts in customerAccounts)
                    {
                        var acctType = accounts.AccountType;
                        var acctBalance = accounts.AccountBalance;
                        var acctStatus = accounts.AccountStatus;
                        var loanAccount = loanAccounts.Where(x => x.CustomerAccount.Id == accounts.Id).FirstOrDefault();
                        if (acctStatus == Status.Inactive.ToString())
                        {
                            break;
                        }
                        switch (acctType)
                        {
                            case AccountType.Savings:
                                if (acctStatus == Status.Inactive.ToString())
                                {
                                    break;
                                }
                                else
                                {
                                    if (acctBalance < sac.MinimumBalance)
                                    {
                                        break;
                                    }

                                    var dailyRate = Math.Round((sac.CreditInterestRate / 360), 2);
                                    var InterestOnSavingsAccount = Math.Round((acctBalance * dailyRate) / 100, 2);
                                    acctBalance += InterestOnSavingsAccount;
                                    acctBalance += InterestOnSavingsAccount;
                                    accounts.AccountBalance = acctBalance;
                                    /*glAccount to debit and credit
                                   debit savingsMirror,current mirror account
                                   credit InterestExpense
                                    */
                                    savingGLAccount.AccountBalance -= InterestOnSavingsAccount;
                                    savingInterestExpenseGL.AccountBalance -= InterestOnSavingsAccount;

                                }
                                break;
                            case AccountType.Current:
                                if (acctStatus == Status.Inactive.ToString())
                                {
                                    break;
                                }
                                else
                                {
                                    if (acctBalance < cac.MinimumBalance)
                                    {
                                        break;
                                    }

                                    var dailyRate = Math.Round(cac.CreditInterestRate / 360, 2);
                                    var interestOnCurrentAccount = Math.Round((acctBalance * dailyRate) / 100);
                                    acctBalance = acctBalance + interestOnCurrentAccount;
                                    accounts.AccountBalance = acctBalance;
                                    currentGLAccount.AccountBalance -= interestOnCurrentAccount;
                                    currentInterestExpenseGL.AccountBalance -= interestOnCurrentAccount;
                                    cotIncomeGL.AccountBalance += cac.COT;
                                    currentGLAccount.AccountBalance -= cac.COT;

                                }
                                break;
                            default:
                                break;
                        }
                        if (loanAccount != null)
                        {

                            if (lac == null)
                            {
                                throw new Exception("Please create loan Account configurations to complete EOD process");
                            }
                            if (loanInterestIncomeGL == null || loanGLAccount == null)
                            {
                                throw new Exception("Please Check creation of all loan gl accounts to enable properly documention of loan process");
                            }

                            var interestOnLoan = (lac.DebitInterestRate * loanAccount.PrincipalAmount * loanAccount.Duration) / (360 * 100);
                            var loanTenureMths = ((double)loanAccount.Duration / 30);
                            var mnthlyInterestOnLoan = interestOnLoan / loanTenureMths;
                            var dailyInterestOnLoan = mnthlyInterestOnLoan / 30;
                            //Debit InterestReceivable
                            //interestReceivable.AccountBalance += dailyInterestOnLoan;
                            //Credit InterestIncome
                            loanInterestIncomeGL.AccountBalance += dailyInterestOnLoan;
                            var amountPayable = loanAccount.PrincipalAmount + interestOnLoan;
                            // if (loanAccount.LoanDueDate == businessStatus.FinancialDate)
                            {
                                if (accounts.AccountBalance < amountPayable)
                                {
                                    //Debit Income(Interest Income)
                                    loanGLAccount.AccountBalance -= interestOnLoan;
                                }
                                else
                                {
                                    accounts.AccountBalance -= amountPayable;
                                    //  loanGLAccount.AccountBalance -= loanAccount.PrincipalAmount;
                                }

                            }
                        }

                        glAccountRepository.Update(cac.CustomerCurrentAccount, cac.CustomerCurrentAccount.Id);
                        glAccountRepository.Update(sac.CustomerSavingAccount, sac.CustomerSavingAccount.Id);
                        glAccountRepository.Update(lac.LoanGL, lac.LoanGL.Id);
                        glAccountRepository.Update(cac.COTIncomeGL, cac.COTIncomeGL.Id);
                        glAccountRepository.Update(cac.InterestExpenseGL, cac.InterestExpenseGL.Id);
                        glAccountRepository.Update(sac.InterestExpenseGL, sac.InterestExpenseGL.Id);
                        //accounts updated
                    }
                    var eod = eodRepo.GetById(1);
                    eod.IsOpen = false;
                    eod.FinancialDate = eodRepo.GetById(1).FinancialDate;
                    var newFinDate = eod.FinancialDate.AddDays(1);
                    eod.FinancialDate = newFinDate;
                    eodRepo.Update(eod, eod.Id);
                    //eod updated
                }
            }

        }
    }
}