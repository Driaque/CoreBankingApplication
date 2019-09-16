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
    public class GLAccountController : Controller
    {
        private GLAccountRepository _glAccountRepository;

        public GLAccountController()
        {
            _glAccountRepository = new GLAccountRepository();
        }
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

            var glAccounts = _glAccountRepository.GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        glAccounts = session.Query<GLAccount>().Where(u => u.GLAccountName.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }

                    switch (sortOrder)
                    {
                        case "name_desc":
                            glAccounts = glAccounts.OrderByDescending(a => a.GLAccountName).ToList();
                            break;
                        case "id_desc":
                            glAccounts = glAccounts.OrderByDescending(a => a.GLCode).ToList();
                            break;
                        default:
                            glAccounts = glAccounts.OrderByDescending(u => u.Id).ToList();
                            break;
                    }
                    int pageSize = 3;
                    int pageNumber = (page ?? 1);
                    return View(glAccounts.ToPagedList(pageNumber, pageSize));                }
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new GLAccount());
        }

        public ActionResult Create()
        {
            var glCategory = new Repository<GLCategory>();
            ViewBag.Catergories = glCategory.GetAll();

            var branchRepository = new Repository<Branch>();
            ViewBag.Branches = branchRepository.GetAll();
            return View(new GLAccount());
        }

        [HttpPost]
        public ActionResult Create(GLAccount glAccount)
        {
            var branchRepository = new Repository<Branch>();
            ViewBag.Branches = branchRepository.GetAll();
            var glCategory = new Repository<GLCategory>();
            ViewBag.Catergories = glCategory.GetAll();
            // if (ModelState.IsValid)
            {
                try
                {

                    //var categories = session.Query<GLCategory>().FirstOrDefault(b => b.Id == glAccount.GLCategory.Id);
                    //glAccount.GLCategory = categories;
                    var glaccounts = _glAccountRepository.Filter(u => u.GLAccountName == glAccount.GLAccountName);

                    if (glaccounts.Count == 0)
                    {
                        glAccount.GLCode = GenerateGLAccountCode(glAccount, glAccount.Id);
                        glAccount.DateAdded = DateTime.Now;
                        glAccount.DateUpdated = DateTime.Now;
                        _glAccountRepository.Save(glAccount);
                    }

                    return RedirectToAction("Index");

                    //ModelState.AddModelError("GLAccountName", "The Account name already exists.");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception.Message;
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var glCategory = new Repository<GLCategory>();
            ViewBag.Catergories = glCategory.GetAll();
            var branchRepository = new Repository<Branch>();
            ViewBag.Branches = branchRepository.GetAll();
            var glAccountRepository = new Repository<GLAccount>();
            return View(glAccountRepository.GetById(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, GLAccount glAccount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var glCategory = new Repository<GLCategory>();
                    ViewBag.Catergories = glCategory.GetAll();
                    var branchRepository = new Repository<Branch>();
                    ViewBag.Branches = branchRepository.GetAll();
                    var glAccountRepository = new Repository<GLAccount>();
                    var oldEntity = glAccountRepository.GetById(glAccount.Id);
                    glAccount.GLCode = oldEntity.GLCode;
                    glAccount.DateAdded = oldEntity.DateAdded;
                    glAccount.DateUpdated = DateTime.Now;
                    glAccountRepository.Update(glAccount, glAccount.Id);

                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception;
                }
            }
            return View(glAccount);
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