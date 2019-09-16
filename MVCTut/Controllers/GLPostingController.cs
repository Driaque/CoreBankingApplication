using Core;
using Data;
using Data.Repository;
using MVCTut.CustomAttributes;
using NHibernate;
using NHibernate.Linq;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class GLPostingController : Controller
    {
        private GLPostingRepository _glPostingRepository;

        public GLPostingController()
        {
            _glPostingRepository = new GLPostingRepository();
        }
        // GET: GLPosting

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc1" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc1" : "Name";
            ViewBag.NameSortParm2 = sortOrder == "Name" ? "name_desc2" : "Name";
            ViewBag.IdSortParm2 = sortOrder == "Id" ? "id_desc2" : "Id";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

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

            var glPostings = _glPostingRepository.GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        glPostings = session.Query<GLPosting>().Where(u => u.GLAccountToDebit.GLAccountName.ToUpper().Contains(searchString.ToUpper()) ||
                        u.GLAccountToCredit.GLAccountName.ToUpper().Contains(searchString.ToUpper()) ||
                        u.GLAccountToCredit.GLCode.Contains(searchString.ToUpper())).ToList();
                    }

                    switch (sortOrder)
                    {
                        case "name_desc1":
                            glPostings = glPostings.OrderByDescending(a => a.GLAccountToDebit.GLAccountName).ToList();
                            break;
                        case "id_desc1":
                            glPostings = glPostings.OrderByDescending(a => a.GLAccountToDebit.GLCode).ToList();
                            break;
                        case "name_desc2":
                            glPostings = glPostings.OrderByDescending(a => a.GLAccountToCredit.GLAccountName).ToList();
                            break;
                        case "id_desc2":
                            glPostings = glPostings.OrderByDescending(a => a.GLAccountToCredit.GLCode).ToList();
                            break;
                        case "date_desc":
                            glPostings = glPostings.OrderByDescending(a => a.TransactionDate).ToList();
                            break;
                        default:
                            glPostings = glPostings.OrderByDescending(u => u.Id).ToList();
                            break;
                    }
                    int pageSize = 3;
                    int pageNumber = (page ?? 1);
                    return View(glPostings.ToPagedList(pageNumber, pageSize));
                }

                //return View(customers.ToList());
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new GLPosting());
        }

        [RestrictPostTransaction]
        public ActionResult Create()
        {
            var glAccountRepository = new Repository<GLAccount>();
            ViewBag.GLAccounts = glAccountRepository.GetAll();

            return View(new GLPosting());
        }

        [RestrictPostTransaction]
        [HttpPost]
        public ActionResult Create(GLPosting glPost)
        {
            var glAccountRepository = new Repository<GLAccount>();
            ViewBag.GLAccounts = glAccountRepository.GetAll();
            // if (ModelState.IsValid)
            {
                try
                {
                    if (glPost.GLAccountToDebit.Id != glPost.GLAccountToCredit.Id)
                    {
                        using (ISession session = NHibernateHelper.Session)
                        {
                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                var Cacc = session.Query<GLAccount>().FirstOrDefault(b => b.Id == glPost.GLAccountToCredit.Id);
                                glPost.GLAccountToCredit = Cacc;
                                var Dacc = session.Query<GLAccount>().FirstOrDefault(b => b.Id == glPost.GLAccountToDebit.Id);
                                glPost.GLAccountToDebit = Dacc;
                                glPost.TransactionDate = DateTime.Now;

                                if (!IsAssetOrExpense(Dacc))
                                {

                                    if (Dacc.AccountBalance >= glPost.PostAmount)
                                    {
                                        Dacc.AccountBalance -= glPost.PostAmount;
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("PostAmount", "Insufficient Balance");
                                    }
                                }
                                else
                                {
                                    Dacc.AccountBalance += glPost.PostAmount;
                                }
                                if (!IsAssetOrExpense(Cacc))
                                {
                                    Cacc.AccountBalance += glPost.PostAmount;
                                }
                                else
                                {
                                    if (Cacc.AccountBalance > glPost.PostAmount)
                                    {
                                        Cacc.AccountBalance -= glPost.PostAmount;
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("PostAmount", "Insufficient Balance");
                                    }
                                }
                                glPost.DateAdded = DateTime.Now;
                                glPost.DateUpdated = DateTime.Now;
                                glAccountRepository.Update(Dacc, Dacc.Id);
                                glAccountRepository.Update(Cacc, Cacc.Id);
                                _glPostingRepository.Save(glPost);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("Id", "You cannot Post to the same account.");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception.Message;
                }
            }
            return View();
        }

        private bool IsAssetOrExpense(GLAccount glAccount)
        {
            var a = glAccount.GLCategory.CategoryType;
            return (a == GLCategoryType.Asset || a == GLCategoryType.Expense);
        }


    }
}